using UnityEngine;
using DG.Tweening;

public class SwordBehavior : Weapon
{
    [SerializeField] private float lifeTime = 0.3f;      // общая длительность удара
    [SerializeField] private float slashDistance = 1.2f; // насколько выносить меч вперёд
    [SerializeField] private float slashArcAngle = 40f;  // дуга (по желанию)

    private Tween _moveTween;
    private Tween _fadeTween;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        // Чистим твины, чтобы не висели
        _moveTween?.Kill();
        _fadeTween?.Kill();
    }

    public override void Initialize()
    {
        base.Initialize();

        if (player == null)
            player = Player.singleton.GetComponent<PlayerMovement>();

        if (_sr != null)
            _sr.color = new Color(1, 1, 1, 1);

        Vector2 dir = player.MovementDirection;

        if (dir.sqrMagnitude < 0.001f)
            dir = player.LastMoveDirection.sqrMagnitude > 0.001f
                ? player.LastMoveDirection
                : Vector2.right;

        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform
            .DOMove(player.transform.position + (Vector3)dir * slashDistance, lifeTime * 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOMove(player.transform.position, lifeTime * 0.5f)
                    .SetEase(Ease.InQuad);
            });

        // 3) плавное исчезновение в конце

        if (_sr != null)
        {
            _fadeTween = _sr.DOFade(0f, lifeTime * 0.4f) // последние 40% времени
                .SetEase(Ease.InQuad)
                .SetDelay(lifeTime * 0.6f);
        }

        // 4) Возврат в пул по окончании
        DOVirtual.DelayedCall(lifeTime, () =>
        {
            if (generator != null) 
                generator.PutObject(this);
        });
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage, transform.position);
        }
    }
}
