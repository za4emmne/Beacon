using UnityEngine;
using System;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _knockbackDistance = 0.4f;
    [SerializeField] private float _knockbackTime = 0.1f;

    [Header("Орда - Поведение")]
    [SerializeField] private float _speedVariance = 0.2f;
    [SerializeField] private float _separationRadius = 1.2f;
    [SerializeField] private float _separationForce = 2f;

    public event Action AnimationRunPlayed;

    private Transform _target;
    private EnemyHealth _health;
    private Vector3 _rotate;
    private bool _isKnockedBack;
    private float _actualSpeed;
    private bool _separationEnabled = true;

    public float Speed => _speed;

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        _target = Player.singleton.transform;
    }

    private void Update()
    {
        if (!_isKnockedBack)
            MoveToTarget();
    }

    private void OnEnable()
    {
        _health.Died += Stop;
    }

    private void OnDisable()
    {
        _health.Died -= Stop;
    }

    public void Init(float speed)
    {
        _speed = speed;
        _actualSpeed = speed * (1f + UnityEngine.Random.Range(-_speedVariance, _speedVariance));
    }

    private void MoveToTarget()
    {
        if (_target == null) return;

        Vector3 direction = (_target.position - transform.position).normalized;

        if (_separationEnabled)
        {
            Vector3 separation = CalculateSeparation();
            direction = (direction + separation * _separationForce).normalized;
        }

        transform.position += direction * _actualSpeed * Time.deltaTime;

        AnimationRunPlayed?.Invoke();
        Flip(_target);
    }

    private Vector3 CalculateSeparation()
    {
        Vector3 separation = Vector3.zero;
        int neighbors = 0;

        for (int i = 0; i < EnemiesGenerator.AllEnemies.Count; i++)
        {
            Enemy other = EnemiesGenerator.AllEnemies[i];
            if (other == null || other.gameObject == gameObject) continue;

            float dist = Vector3.Distance(transform.position, other.transform.position);

            if (dist < _separationRadius && dist > 0.01f)
            {
                Vector3 away = transform.position - other.transform.position;
                separation += away / dist;
                neighbors++;
            }

            if (neighbors >= 3) break;
        }

        return neighbors > 0 ? separation / neighbors : Vector3.zero;
    }

    public void KnockbackFromPlayer(Vector3 playerPos)
    {
        Vector3 dir = (transform.position - playerPos).normalized;
        StartCoroutine(KnockbackRoutine(dir));
    }

    private IEnumerator KnockbackRoutine(Vector3 dir)
    {
        _isKnockedBack = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + dir * _knockbackDistance;
        float t = 0f;

        while (t < _knockbackTime)
        {
            t += Time.deltaTime;
            float k = t / _knockbackTime;
            transform.position = Vector3.Lerp(startPos, endPos, k);
            yield return null;
        }

        _isKnockedBack = false;
    }

    private void Flip(Transform target)
    {
        if ((target.position.x - transform.position.x) < 0)
            _rotate.y = 180;
        else
            _rotate.y = 0;

        transform.rotation = Quaternion.Euler(_rotate);
    }

    private void Stop() => _speed = 0;
}
