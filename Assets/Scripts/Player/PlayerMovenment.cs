using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovenment : MonoBehaviour
{
    private const string AnimationNameRun = "Run";
    private const string AnimationNameIdle = "Idle";

    [SerializeField] private float _speed = 3;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private bool _isFaceRight = true;
    private float _horizontalMove;
    private float _verticalMove;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _speed;
        _verticalMove = Input.GetAxisRaw("Vertical") * _speed;

        _rigidbody2D.velocity = new Vector2(_horizontalMove, _verticalMove);

        if (_horizontalMove < 0 && _isFaceRight)
        {
            Flip();
        }
        else if (_horizontalMove > 0 && !_isFaceRight)
        {
            Flip();
        }

        _animator.SetFloat(AnimationNameRun, Mathf.Abs(_verticalMove + _horizontalMove));
    }

    private void Flip()
    {
        _isFaceRight = !_isFaceRight;

        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
}
