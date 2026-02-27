using UnityEngine;

public class MalletBehavior : Weapon
{
    [SerializeField] private float _rotationSpeed = 360f;
    [SerializeField] private float _upwardForce = 8f;
    [SerializeField] private float _forwardForce = 5f;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _startPosition;
    private bool _hasHitGround;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void Initialize()
    {
        base.Initialize();

        _startPosition = transform.position;
        _hasHitGround = false;

        if (_rigidbody2D != null)
        {
            _rigidbody2D.gravityScale = 1f;
            LaunchMallet();
        }
    }

    private void LaunchMallet()
    {
        Vector2 direction = GetPlayerDirection();
        Vector2 force = direction * _forwardForce + Vector2.up * _upwardForce;
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }

    private Vector2 GetPlayerDirection()
    {
        if (Player.singleton == null)
            return Vector2.right;

        var playerMovement = Player.singleton.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            if (playerMovement.MovementDirection.sqrMagnitude > 0.001f)
                return playerMovement.MovementDirection.normalized;
            if (playerMovement.LastMoveDirection.sqrMagnitude > 0.001f)
                return playerMovement.LastMoveDirection.normalized;
        }
        return Vector2.right;
    }

    private void Update()
    {
        if (_rigidbody2D != null)
        {
            transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);

            if (!_hasHitGround && _rigidbody2D.linearVelocity.y < -0.1f)
            {
                if (transform.position.y < _startPosition.y - 2f)
                {
                    _hasHitGround = true;
                }
            }

            if (_hasHitGround && _rigidbody2D.linearVelocity.magnitude < 0.5f)
            {
                ReturnToPool();
            }
        }
    }

    private void ReturnToPool()
    {
        if (generator != null)
            generator.PutObject(this);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage, transform.position);
        }

        if (collision.TryGetComponent<ObjectKiller>(out ObjectKiller killer))
        {
            ReturnToPool();
        }
    }
}
