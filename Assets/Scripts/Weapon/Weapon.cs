using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData data;
    [SerializeField] protected float damage;
    [SerializeField] protected float delay;
    [SerializeField] protected float speed;

    protected Transform _target;
    protected PlayerMovement player;
    protected GeneratorWeapon generator;
    protected Vector2 direction;

    private void OnDisable()
    {
        // ДОБАВЛЕНЫ ПРОВЕРКИ НА NULL
        if (data != null && data.weaponType == TypeWeapon.Ranged)
        {
            if (generator != null)
            {
                generator.RemoveProjectileFromList(this);
            }
        }
    }

    public virtual void Initialize()
    {
        if (player == null)
            player = Player.singleton.GetComponent<PlayerMovement>();

        _target = null;

        // ПРОВЕРКА: data может быть null для LightningWeapon
        if (data != null)
        {
            damage = data.CurrentDamage;
            delay = data.CurrentDelay;
            speed = data.CurrentSpeed;
        }
    }

    public void InitGenerator(GeneratorWeapon gen)
    {
        generator = gen;

        // ПРОВЕРКА: добавляем только если генератор существует
        if (generator != null)
        {
            generator.AddProjectileOnList(this);
        }
    }

    public void SetZeroDirection()
    {
        direction = Vector2.zero;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);

            if (data != null && data.weaponType == TypeWeapon.Ranged)
            {
                if (generator != null)
                    generator.PutObject(this);
            }
        }

        if (collision.TryGetComponent<ObjectKiller>(out ObjectKiller killer))
        {
            if (data != null && data.weaponType == TypeWeapon.Ranged)
            {
                if (generator != null)
                    generator.PutObject(this);
            }
        }

        if (collision.TryGetComponent<Loot>(out Loot loot))
        {
            if (data != null && data.weaponType == TypeWeapon.Melee)
            {
                loot.Disapear();
            }
        }
    }
}
