using System;
using UnityEngine;
//ÄÎÄÅËÀÒÜ
public class WeaponController : MonoBehaviour
{
    [SerializeField] protected GameObject _prefab;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _cooldownDuration;
    [SerializeField] protected float _currentCooldown;

    protected PlayerMovenment playerMovenment;

    public float Speed => _speed;

    protected virtual void Start()
    {
        _currentCooldown = _cooldownDuration;
        playerMovenment = Player.singleton.GetComponent<PlayerMovenment>();
    }

    private void Update()
    {
        _currentCooldown -= Time.deltaTime;

        if (_currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        _currentCooldown = _cooldownDuration;
    }
}
