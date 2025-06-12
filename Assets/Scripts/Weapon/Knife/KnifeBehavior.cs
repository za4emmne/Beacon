using UnityEngine;

public class KnifeBehavior : Weapon
{
    private Vector2 _direction;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        base.Initialize();
    }

    private void Update()
    {
        transform.Translate(_direction * speed * Time.deltaTime, Space.World);
    }

    public override void Initialize()
    {
        base.Initialize();

        if (player.LastDirection < 0)
        {
            _spriteRenderer.flipX = true;
            _direction = Vector2.left;
        }
        else
        {
            _spriteRenderer.flipX = false;
            _direction = Vector2.right;
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
            Deactivate();
        }

        if (collision.TryGetComponent<ObjectKiller>(out ObjectKiller killer))
        {
            if (data.weaponType == TypeWeapon.Ranged)
            {
                Deactivate();
                generator.PutObject(this);
            }
        }
    }
}
