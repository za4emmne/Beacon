using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    public event Action AnimationRunPlayed;

    private Transform _target;
    private EnemyHealth _health;
    private Vector3 _rotate;

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        _target = Player.singleton.transform;
        //_speed = UnityEngine.Random.Range(0.1f, 0.2f);
    }

    private void Update()
    {
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
    }

    private void MoveToTarget()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                        _target.position, _speed * Time.deltaTime);
            AnimationRunPlayed?.Invoke();
            Flip(_target);
        }
    }

    private void Flip(Transform target) //изменить на флип игрока через трансформ
    {
        if ((target.position.x - transform.position.x) < 0)
        {
            _rotate.y = 180;
            transform.rotation = Quaternion.Euler(_rotate);
            //_spriteRenderer.flipX = true;
        }
        else
        {
            _rotate.y = 0;
            transform.rotation = Quaternion.Euler(_rotate);
            //_spriteRenderer.flipX = false;
        }
    }

    private void Stop()
    {
        _speed = 0;
    }
}