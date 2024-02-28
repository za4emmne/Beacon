using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMovenment : MonoBehaviour
{
    [SerializeField] private float _speed = 3;

    public UnityEvent AnimationRun;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private float _horizontalMove;
    private float _verticalMove;

    public float HorizontalMove => _horizontalMove;
    public float VerticalMove => _verticalMove;


    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _speed;
        _verticalMove = Input.GetAxisRaw("Vertical") * _speed;

        _rigidbody2D.velocity = new Vector2(_horizontalMove, _verticalMove);

        if (_horizontalMove < 0)
        {
            _spriteRenderer.flipX = true;
        }
        if (_horizontalMove > 0)
        {
            _spriteRenderer.flipX = false;
        }

        AnimationRun?.Invoke();
        //_animator.SetFloat(AnimationNameRun, Mathf.Abs(_verticalMove + _horizontalMove));
    }
}
