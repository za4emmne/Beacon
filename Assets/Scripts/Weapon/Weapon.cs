using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData _data;
    [SerializeField] private float damage;
    [SerializeField] protected float _delay;
    [SerializeField] protected float speed;
 

    //protected virtual void Start()
    //{

    //}

    public virtual void Initialize()
    {
        damage = _data.damage;
        _delay = _data.delay;
        speed = _data.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
