using UnityEngine;
using System;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _knockbackDistance = 0.4f;
    [SerializeField] private float _knockbackTime = 0.1f;

    public event Action AnimationRunPlayed;

    private Transform _target;
    private EnemyHealth _health;
    private Vector3 _rotate;
    private bool _isKnockedBack;

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

    public void Init(float speed) => _speed = speed;

    private void MoveToTarget()
    {
        if (_target == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            _target.position,
            _speed * Time.deltaTime);

        AnimationRunPlayed?.Invoke();
        Flip(_target);
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
