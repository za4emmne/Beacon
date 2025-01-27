using System;
using UnityEngine;

public class EnemyViewer : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Sprite _image;

    private EnemyManager _manager;


    private void Start()
    {
        _image = _enemy.Sprite;
    }

    public void Initialize(EnemyManager enemyManager)
    {
        _manager = enemyManager;
    }

    public void OnRelease()
    {
        _manager.OnRelease(this);
    }
}
