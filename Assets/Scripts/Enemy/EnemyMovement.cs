using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    public event Action AnimationRunPlayed;

    private SpriteRenderer _spriteRenderer;
    private Transform _target;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _speed = UnityEngine.Random.Range(0.1f, 0.2f);
    }

    private void Update()
    {
        MoveToTarget();
    }

    public void TakeTargetPosition(Transform target)
    {
        _target = target;        
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

    private void Flip(Transform target)
    {
        if ((target.position.x - transform.position.x) < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }
}