using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, IResettable
{
    [SerializeField] private float height;
    [SerializeField] private List<GameObject> lockEnemies;
    [SerializeField] private Transform wall;
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
        initialY = visual.localPosition.y;
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
        visual.DOLocalMoveY(initialY + height, 1f).SetEase(Ease.OutSine);
        wall.localPosition = new Vector3(wall.localPosition.x, initialY + height, wall.localPosition.z);
    }

    private void Close()
    {
        visual.DOLocalMoveY(initialY, 1f);
        wall.localPosition = new Vector3(wall.localPosition.x, initialY, wall.localPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layer.player)
        {
            Close();
        }
    }
}
