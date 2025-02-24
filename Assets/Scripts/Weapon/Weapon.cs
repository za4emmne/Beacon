using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData _data;
    [SerializeField] private float damage;
    [SerializeField] protected float _delay;
    [SerializeField] protected float speed;
    [SerializeField] protected int level;

    private PlayerMovenment _player;

    private void Start()
    {
        _player = Player.singleton.GetComponent<PlayerMovenment>();
    }

    public virtual void Initialize()
    {
        damage = _data.damage;
        _delay = _data.delay;
        speed = _data.speed;
        level = _data.level;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    //protected void Flip(bool isChanged)
    //{
    //    if (isChanged && transform.position.x > 0)
    //    {
    //        _renderer.flipX = isChanged;
    //    }
    //    else
    //    {
    //        transform.position *= -1;
    //    }

    //}
}
