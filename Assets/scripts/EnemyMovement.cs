using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class EnemyMovement : MonoBehaviour
{
    private const string AnimationNameRun = "Run";

    [SerializeField] private float _speed = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Player _player;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            _player.transform.position, _speed * Time.deltaTime);
        AnimationRun();
        Flip();
    }

    private void AnimationRun()
    {
        _animator.SetTrigger(AnimationNameRun);
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