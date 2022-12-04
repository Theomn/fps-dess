using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializableList<T>
{
    public List<T> list;
}

[Serializable]
public class Checkpoint
{
    public int id;
    public Vector3 position;
    public enum State
    {
        Locked,
        Unlocked,
        Active
    }
    public State state;

    public Checkpoint(int id, Vector3 position, State state)
    {
        this.id = id;
        this.position = position;
        this.state = state;
    }
}

public class ProgressionManager : SingletonMonoBehaviour<ProgressionManager>
{
    [SerializeField] private bool rememberActiveCheckpointOnQuit;

    private SerializableList<Checkpoint> checkpoints;
    private readonly string filename = "/save.json";

    protected override void Awake()
    {
        base.Awake();

        // Retrieve progression from save file
        if (File.Exists(Application.persistentDataPath + filename))
        {
            string json = File.ReadAllText(Application.persistentDataPath + filename);
            checkpoints = JsonUtility.FromJson<SerializableList<Checkpoint>>(json);
            Debug.Log(checkpoints.list.Count + " checkpoints loaded from " + Application.persistentDataPath + filename);
        }
        else
        {
            checkpoints = new SerializableList<Checkpoint>();
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        if (!rememberActiveCheckpointOnQuit)
        {
            foreach (Checkpoint checkpoint in checkpoints.list)
            {
                if (checkpoint.state == Checkpoint.State.Active)
                {
                    checkpoint.state = Checkpoint.State.Unlocked;
                }
            }
        }
        string json = JsonUtility.ToJson(checkpoints);
        File.WriteAllText(Application.persistentDataPath + filename, json);
        Debug.Log(checkpoints.list.Count + " checkpoints saved to " + Application.persistentDataPath + filename);
    }

    public void Register(int id, Vector3 position)
    {
        if (id == 0)
        {
            Debug.LogWarning("Tried to register a checkpoint with id 0.");
            return;
        }
        if (!checkpoints.list.Exists(cp => cp.id == id))
        {
            checkpoints.list.Add(new Checkpoint(id, position, Checkpoint.State.Locked));
        }
    }

    public void ActivateCheckpoint(int id)
    {
        if (id == 0)
        {
            return;
        }
        Debug.Log("Checkpoint " + id + " reached.");
        foreach(Checkpoint checkpoint in checkpoints.list)
        {
            if(checkpoint.id == id)
            {
                checkpoint.state = Checkpoint.State.Active;
            }
            else if (checkpoint.state == Checkpoint.State.Active)
            {
                checkpoint.state = Checkpoint.State.Unlocked;
            }
        }
    }

    public Vector3 GetActiveCheckpointPosition()
    {
        foreach (Checkpoint checkpoint in checkpoints.list)
        {
            if (checkpoint.state == Checkpoint.State.Active)
            {
                return checkpoint.position;
            }
        }
        return Vector3.zero;
    }
}
