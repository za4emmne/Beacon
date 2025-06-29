using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData data;
    [SerializeField] protected float damage;
    [SerializeField] protected float delay;
    [SerializeField] protected float speed;

    protected PlayerMovement player;
    protected GeneratorWeapon generator;
    protected Vector2 direction;

    private void Start()
    {

    }

    public virtual void Initialize()
    {
        if (player == null)
            player = Player.singleton.GetComponent<PlayerMovement>();

        damage = data.CurrentDamage;
        delay = data.CurrentDelay;
        speed = data.CurrentSpeed;
    }

    public void InitGenerator(GeneratorWeapon generator)
    {
        this.generator = generator;
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

            if (data.weaponType == TypeWeapon.Ranged)
            {
                generator.PutObject(this);
            }
        }
        if (collision.TryGetComponent<ObjectKiller>(out ObjectKiller killer))
        {
            if (data.weaponType == TypeWeapon.Ranged)
            {
                generator.PutObject(this);
            }
        }
    }
}
