using System.Collections;
using UnityEngine;

public class MagicBall : Weapon
{
    [Header("Orbit Settings")]
    [SerializeField] private float _orbitRadius = 1f;
    [SerializeField] private float _orbitSpeed = 90f;
    [SerializeField] private bool _clockwise = true;
    [SerializeField] private float _lifetime = 3f;

    [Header("Starting Position")]
    [SerializeField] private float _startAngle = 0f;

    private float _timeAlive = 0f;
    private float _currentAngle;
    private Coroutine _coroutine;
    private Coroutine _searchTargetCoroutine;
    private float _detectedRadius;
    private LayerMask _targetLayer = 1 << 8;

    public override void Initialize()
    {
        base.Initialize();
        _detectedRadius = data.CurrentAttackRange;

        if (_coroutine == null && this.gameObject.activeSelf)
            _coroutine = StartCoroutine(MainMoving());
        else
        {
            this.gameObject.SetActive(true);
            StopCoroutine();
            _coroutine = StartCoroutine(MainMoving());
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage, transform.position);

            if (data.weaponType == TypeWeapon.Ranged)
            {
                StopCoroutine();
                generator.PutObject(this);
            }
        }
        if (collision.TryGetComponent<ObjectKiller>(out ObjectKiller killer))
        {
            if (data.weaponType == TypeWeapon.Ranged)
            {
                StopCoroutine();
                generator.PutObject(this);
            }
        }
    }

    private void StopCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        if (_searchTargetCoroutine != null)
        {
            StopCoroutine(_searchTargetCoroutine);
            _searchTargetCoroutine = null;
        }
    }

    private IEnumerator MainMoving()
    {
        yield return FirstMoving();

        while (_lifetime > _timeAlive)
        {
            _timeAlive += Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
            yield return null;
        }

        StopCoroutine();
        generator.PutObject(this);
    }

    private IEnumerator FirstMoving()
    {
        if (generator == null)
        {
            Debug.LogError("Generator не назначен для MagicBall!");
            yield break;
        }

        while (_target == null)
        {
            Vector3 offset = new Vector3(
                Mathf.Cos(_currentAngle * Mathf.Deg2Rad) * _orbitRadius,
                Mathf.Sin(_currentAngle * Mathf.Deg2Rad) * _orbitRadius,
                0f
            );

            transform.position = player.transform.position + offset;

            float direction = _clockwise ? -1f : 1f;
            _currentAngle += _orbitSpeed * direction * Time.deltaTime;

            if (_currentAngle >= 360f) _currentAngle -= 360f;

            if (_currentAngle < 0f) _currentAngle += 360f;

            Enemy nearest = generator.FindNearestEnemy();
            if (nearest != null)
            {
                _target = nearest.transform;
                yield break; // моментально завершаем вращение
            }

            yield return null;
        }
    }
}
