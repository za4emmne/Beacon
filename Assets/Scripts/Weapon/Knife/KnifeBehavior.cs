using UnityEngine;

public class KnifeBehavior : Weapon
{
    private Vector2 _direction;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _lastMovementDirection = Vector2.right;

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
        transform.Translate(_lastMovementDirection * speed * Time.deltaTime, Space.World);
    }

    public override void Initialize()
    {
        base.Initialize();

        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (movementInput.magnitude > 0.1f)
        {
            _lastMovementDirection = movementInput.normalized;
        }

        float angle = Mathf.Atan2(_lastMovementDirection.y, _lastMovementDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //if (player.LastDirection < 0)
        //{
        //    _spriteRenderer.flipX = true;
        //    _direction = Vector2.left;
        //}
        //else
        //{
        //    _spriteRenderer.flipX = false;
        //    _direction = Vector2.right;
        //}
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
            generator.PutObject(this);
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
