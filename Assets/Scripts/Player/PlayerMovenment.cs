using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMovenment : MonoBehaviour
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
    private Vector3 _rotate;

    public event Action Run;
    public event Action<bool> Flip;

    public float HorizontalMove => _horizontalMove;
    public float VerticalMove => _verticalMove;


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
            //_rotate.y = 180;
            ////transform.rotation = Quaternion.Euler(_rotate);
            //_spriteRenderer.flipX = true;
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        if (_horizontalMove > 0 || _joystick.Horizontal > 0)
        {
            //_rotate.y = 0;
            //transform.rotation = Quaternion.Euler(_rotate);
            //_spriteRenderer.flipX = false;
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        Run?.Invoke();
    }

    private void Joy()
    {
        _rigidbody2D.velocity = new Vector3(_joystick.Horizontal * _speed, _joystick.Vertical * _speed);
        JoystickCurrentHorizontal = _joystick.Horizontal;
        JoystickCurrentVertical = _joystick.Vertical;
    }

    private void Keyboard()
    {
        _horizontalMove = Input.GetAxisRaw(NameDirectionHorizontal) * _speed;
        _verticalMove = Input.GetAxisRaw(NameDirectionVertical) * _speed;
        _rigidbody2D.velocity = new Vector2(_horizontalMove, _verticalMove);
    }
}
