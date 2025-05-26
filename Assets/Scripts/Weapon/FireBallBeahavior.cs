using UnityEngine;

public class FireBallBeahavior : Weapon
{
    private Vector2 _direction;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.Translate(_direction * speed * Time.deltaTime, Space.World);
    }

    private void Start()
    {
        base.Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        if (player.GetDirection().x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (player.GetDirection().x < 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (player.GetDirection().x == 0)
        {
            int random = Random.Range(0, 2);

            if (random == 0)
                _spriteRenderer.flipX = false;
            else
                _spriteRenderer.flipX = true;
        }

        _direction = new Vector2(player.GetDirection().x, 0);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
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
