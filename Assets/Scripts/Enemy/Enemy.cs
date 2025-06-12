using System;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyAttacked))]

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyData Data;
    //[SerializeField] private Sprite _image;
    [SerializeField] private float _health;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    private EnemiesGenerator _manager;
    private EnemyAttacked _attackController;
    private EnemyHealth _healthController;
    private EnemyMovement _movementController;

    private void Awake()
    {
        _attackController = GetComponent<EnemyAttacked>();
        _healthController = GetComponent<EnemyHealth>();
        _movementController = GetComponent<EnemyMovement>();

        if (!_attackController) Debug.LogError("EnemyAttacked component missing!");
        if (!_healthController) Debug.LogError("EnemyHealth component missing!");
        if (!_movementController) Debug.LogError("EnemyMovement component missing!");
    }

    public void Initialize(EnemyData data, EnemiesGenerator enemyManager)
    {
        if (data == null)
        {
            Debug.LogError("EnemyData is null!");
            return;
        }

        if (_attackController == null || _healthController == null || _movementController == null)
        {
            Debug.LogError("One of the required components is missing!");
            return;
        }

        Data = data;  
        _manager = enemyManager;
        _health = data.Health;
        _damage = data.Damage;
        _speed = data.Speed;

        _attackController.Init(_damage);
        _movementController.Init(_speed);
        _healthController.Init(_health);

    }

    public void OnRelease()
    {
        _manager.PutObject(this);
    }
}
