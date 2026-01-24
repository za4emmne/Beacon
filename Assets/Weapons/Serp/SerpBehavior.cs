using UnityEngine;

public class SerpBehavior : Weapon
{
    [SerializeField] private float maxLifeTime = 3f;
    [SerializeField] private float returnDelay = 0.8f;
    [SerializeField] private float rotationSpeed = 360f; // градусы в секунду

    private float lifeTimer;
    private bool isReturning;
    private float _randomValueRotation;

    public override void Initialize()
    {
        base.Initialize();

        lifeTimer = maxLifeTime;
        isReturning = false;

        Vector2 moveDir = player != null ? player.MovementDirection : Vector2.right;
        if (moveDir == Vector2.zero)
            moveDir = Vector2.right;

        direction = moveDir.normalized;
        _randomValueRotation = Random.Range(3f, 4f);
    }

    private void Update()
    {
        // двигаем снаряд
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // вращаем вокруг оси Z (вперёд/назад на экране)
        transform.Rotate(0f, 0f, rotationSpeed * _randomValueRotation * Time.deltaTime);

        lifeTimer -= Time.deltaTime;

        if (!isReturning && lifeTimer <= maxLifeTime - returnDelay && player != null)
        {
            isReturning = true;
            Vector2 toPlayer = (player.transform.position - transform.position).normalized;
            direction = toPlayer;
        }

        if (lifeTimer <= 0f && data != null && data.weaponType == TypeWeapon.Ranged)
        {
            if (generator != null)
                generator.PutObject(this);
        }
    }
}
