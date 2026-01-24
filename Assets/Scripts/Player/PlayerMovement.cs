using System;
using System.Collections;
using UnityEngine;

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
    [SerializeField] private float _knockbackDistance = 0.4f;
    [SerializeField] private float _knockbackTime = 0.1f;

    public float VerticalMove;
    public float HorizontalMove;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private float _lastDirection;
    private bool _isMobile;
    private bool _isKnockedBack;
    private Vector2 _input;

    public event Action Run;
    public event Action<bool> Flip;

    public float LastDirection => _lastDirection;
    public Vector2 MovementDirection => _input;
    public Vector2 LastMoveDirection { get; private set; } = Vector2.right;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _isMobile = Application.isMobilePlatform;   // ВКЛЮЧАЕМ
                                                    // _isMobile = true;
        if (_joystick != null)
            _joystick.gameObject.SetActive(_isMobile);
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
            return;

        if (_isMobile && _joystick != null)
        {
            _input = new Vector2(_joystick.Horizontal, _joystick.Vertical);
        }
        else
        {
            _input = new Vector2(
                Input.GetAxisRaw(NameDirectionHorizontal),
                Input.GetAxisRaw(NameDirectionVertical)
            );
        }

        _input = Vector2.ClampMagnitude(_input, 1f); // нормализация

        if (_input.sqrMagnitude > 0.001f)
        {
            LastMoveDirection = _input.normalized;
        }

        _rigidbody2D.linearVelocity = _input * _speed;

        JoystickCurrentHorizontal = _input.x;
        JoystickCurrentVertical = _input.y;

        HorizontalMove = _input.x;
        VerticalMove = _input.y;

        if (_rigidbody2D.linearVelocity.sqrMagnitude > 0.01f)
            Run?.Invoke();
    }

    private void Update()
    {
        float currentHorizontal = _input.x;

        if (currentHorizontal < -0.1f)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
            _lastDirection = -1;
        }
        else if (currentHorizontal > 0.1f)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            _lastDirection = 1;
        }
    }

    public void Initialize(FixedJoystick joystick)
    {
        if (joystick == null)
        {
            Debug.LogWarning("Попытка инициализировать джойстик с null значением");
            return;
        }

        _joystick = joystick;
        _joystick.gameObject.SetActive(_isMobile);
    }

    public void UpgradeSpeed(float count)
    {
        if (count <= 0)
        {
            Debug.LogWarning("Значение count должно быть больше 0");
            return;
        }

        _speed += (_speed / count);
    }

    private void Joy()
    {
        // Получаем значения джойстика
        float horizontal = _joystick.Horizontal;
        float vertical = _joystick.Vertical;

        // Применяем движение
        _rigidbody2D.linearVelocity = new Vector2(horizontal * _speed, vertical * _speed);

        // Сохраняем текущие значения для публичного доступа
        JoystickCurrentHorizontal = horizontal;
        JoystickCurrentVertical = vertical;
    }

    private void Keyboard()
    {
        float horizontal = Input.GetAxisRaw(NameDirectionHorizontal);
        float vertical = Input.GetAxisRaw(NameDirectionVertical);
        VerticalMove = vertical;
        HorizontalMove = horizontal;

        _rigidbody2D.linearVelocity = new Vector2(horizontal * _speed, vertical * _speed);

        // Обновляем значения для совместимости
        JoystickCurrentHorizontal = horizontal;
        JoystickCurrentVertical = vertical;
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
}
