using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] PlayerAnimation _animation;
    [SerializeField] WeaponData _currentWeapon;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _attackPoint;

    private float _lastAttackTime = 0f;

    private void Update()
    {
        if (Time.time >= _lastAttackTime + _currentWeapon.delay)
        {
            Attack();
        }

        if (_currentWeapon.weaponType == TypeWeapon.Ranged && Time.time >= _lastAttackTime + _currentWeapon.delay)
        {
            _animation.OnAttackAnimation();
            Shoot();
        }
    }

    private void Attack()
    {
        _lastAttackTime = Time.time;

        if (_currentWeapon.weaponType == TypeWeapon.Melee)
        {
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _currentWeapon.attackRange, _enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(_currentWeapon.damage);
            }
        }
    }

    private void Shoot()
    {
        _lastAttackTime = Time.time;

        if (_currentWeapon.Prefab != null)
        {

            GameObject projectile = Instantiate(_currentWeapon.Prefab, _attackPoint.position, _attackPoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = _attackPoint.up * _currentWeapon.speed;
            }
        }
    }
}
