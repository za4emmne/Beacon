using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    public event Action AnimationRunPlayed;

    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Player _player;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _speed = UnityEngine.Random.Range(0.1f, 0.2f);
    }

    private void Update()
    {
        if (_player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                        _player.transform.position, _speed * Time.deltaTime);
            AnimationRunPlayed?.Invoke();
            Flip();
        }
    }

    private void Flip()
    {
        if ((_player.transform.position.x - transform.position.x) < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }
}