using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//1.Урон
//2.Частота атака(время перезарядки)
//3.
public class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject[] _prefabs;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _cooldownDuration;
    [SerializeField] protected float _currentCooldown;

    protected void Start()
    {
        _currentCooldown = _cooldownDuration;
    }

    protected void Update()
    {
        _currentCooldown -= Time.deltaTime;

        if(_currentCooldown <= 0 )
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        _currentCooldown = _cooldownDuration;
    }
}
