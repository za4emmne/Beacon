using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMovement : MonoBehaviour
{
    public float JoystickCurrentVertical;
    public float JoystickCurrentHorizontal;

    private const string NameDirectionHorizontal = "Horizontal";
    private const string NameDirectionVertical = "Vertical";

    [SerializeField] private float _speed = 3;
    [SerializeField] private FixedJoystick _joystick;

    private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private float _horizontalMove;
    private float _verticalMove;
    private Vector2 _direction;
    private float _lastDirection;

    public event Action Run;
    public event Action<bool> Flip;

    public float HorizontalMove => _horizontalMove;
    public float VerticalMove => _verticalMove;
    public float LastDirection => _lastDirection;


    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            Joy();
        }
        else
        {
            _joystick.gameObject.SetActive(false);
            Keyboard();
        }

        if (_horizontalMove < 0 || _joystick.Horizontal < 0)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        if (_horizontalMove > 0 || _joystick.Horizontal > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        _lastDirection = _horizontalMove;
        Run?.Invoke();
    }

    public void Initialize(FixedJoystick joystick)
    {
        _joystick = joystick;
    }

    public Vector2 GetDirection()
    {
        _direction = new Vector2(
    Input.GetAxisRaw("Horizontal"),
    Input.GetAxisRaw("Vertical")
).normalized;

        return _direction;
    }

    private void Joy()
    {
        _rigidbody2D.linearVelocity = new Vector3(_joystick.Horizontal * _speed, _joystick.Vertical * _speed);
        JoystickCurrentHorizontal = _joystick.Horizontal;
        JoystickCurrentVertical = _joystick.Vertical;
    }

    private void Keyboard()
    {
        _horizontalMove = Input.GetAxisRaw(NameDirectionHorizontal) * _speed;
        _verticalMove = Input.GetAxisRaw(NameDirectionVertical) * _speed;
        _rigidbody2D.linearVelocity = new Vector2(_horizontalMove, _verticalMove);
    }
}
