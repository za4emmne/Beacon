using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyAttacked))]

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemy;
    //[SerializeField] private Sprite _image;
    [SerializeField] private float _health;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    private EnemiesGenerator _manager;
    private EnemyAttacked _attackController;
    private EnemyHealth _healthController;
    private EnemyMovement _movementController;

    public Enemy()
    {
        _attackController = new(); 
        _healthController = new();
        _movementController = new();
    }

    private void Awake()
    {
        _attackController = GetComponent<EnemyAttacked>();
        _healthController = GetComponent<EnemyHealth>();
        _movementController = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        _health = _enemy.Health;
        _damage = _enemy.Damage;
        _speed = _enemy.Speed;

        _attackController.Init(_damage);
        _movementController.Init(_speed);
        _healthController.Init(_health);
    }

    public void Initialize(EnemiesGenerator enemyManager)
    {
        _manager = enemyManager;
        _health = _enemy.Health;
        _damage = _enemy.Damage;
        _speed = _enemy.Speed;

        _attackController.Init(_damage);
        _movementController.Init(_speed);
        _healthController.Init(_health);

    }

    public void OnRelease()
    {
        _manager.PutObject(this);
    }
}
