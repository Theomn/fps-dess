using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyDoor : MonoBehaviour, IResettable
{
    [SerializeField] private float height;
    [SerializeField] private List<GameObject> lockEnemies;
    [SerializeField] private GameObject doorWall;
    [SerializeField] private Transform visual;

    private List<Enemy> enemies;
    private int lockCount;
    private float initialY;

    private void Awake()
    {
        enemies = new List<Enemy>();
        foreach(GameObject enemyObject in lockEnemies)
        {
            enemies.AddRange(enemyObject.GetComponentsInChildren<Enemy>());
        }
        foreach (Enemy enemy in enemies)
        {
            enemy.Register(this);
        }
        initialY = visual.position.y;
        Reset();
    }

    public void Reset()
    {
        lockCount = enemies.Count;
        if (lockCount <= 0)
        {
            Open();
        }
        else
        {
            Debug.Log("Door locked with " + lockCount + " enemies.");
            Close();
        }
    }

    public void DecrementLock()
    {
        lockCount--;
        if (lockCount <= 0)
        {
            Open();
        }
    }

    private void Open()
    {
        visual.DOMoveY(initialY + height, 1f).SetEase(Ease.OutSine);
        doorWall.SetActive(false);
    }

    private void Close()
    {
        visual.DOMoveY(initialY, 1f);
        doorWall.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.player)
        {
            Close();
        }
    }
}
