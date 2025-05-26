using DG.Tweening;
using UnityEngine;

public class KnifeBehavior : Weapon
{
    private Vector2 _direction;
    private SpriteRenderer _spriteRenderer;
    private Tween flightTween;

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
        //transform.Translate(_direction * speed * Time.deltaTime, Space.World);
    }

    public override void Initialize()
    {
        base.Initialize();

        if (player.GetDirection().x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        if(player.GetDirection().x > 0)
        {
            _direction = Vector2.right;
        }
        else
        {
            _direction = Vector2.left;
        }


        flightTween = transform
           .DOMove((Vector2)transform.position + _direction * speed * 5f, 5f)
           .SetEase(Ease.Linear)
           .OnComplete(Deactivate);
    }

    public void Deactivate()
    {
        flightTween?.Kill();  // Останавливаем твин
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
