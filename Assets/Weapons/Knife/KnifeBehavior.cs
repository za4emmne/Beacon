using UnityEngine;

public class KnifeBehavior : Weapon
{
    private Vector2 _direction = Vector2.right;

    private void OnEnable()
    {
        // если направление ещЄ не задано Ц инициируем обычной логикой
        if (_direction == Vector2.zero)
            Initialize();
    }

    private void Update()
    {
        transform.Translate(_direction * speed * Time.deltaTime, Space.World);
    }

    public override void Initialize()
    {
        base.Initialize();

        if (player == null)
            player = Player.singleton.GetComponent<PlayerMovement>();

        Vector2 moveDir = player.MovementDirection;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            _direction = moveDir.normalized;
        }
        else
        {
            Vector2 last = player.LastMoveDirection;
            if (last.sqrMagnitude > 0.001f)
                _direction = last.normalized;
            else
                _direction = Vector2.right;
        }

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Ќќ¬џ…: вызываетс€ генератором дл€ залпа
    public void InitializeWithDirection(Vector2 dir)
    {
        base.Initialize();

        _direction = dir.normalized;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage, transform.position);
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
