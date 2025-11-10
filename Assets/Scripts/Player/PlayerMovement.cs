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
    [SerializeField] private DynamicJoystick _joystick;

    public float VerticalMove;
    public float HorizontalMove;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private float _lastDirection;
    private bool _isMobile;

    public event Action Run;
    public event Action<bool> Flip;

    public float LastDirection => _lastDirection;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //#if UNITY_EDITOR
        //           _isMobile = true;
        //#else
        //            _isMobile = Application.isMobilePlatform;
        //#endif

        //_isMobile = Application.isMobilePlatform;
        // Настраиваем джойстик для платформы
        if (_joystick != null)
        {
            _joystick.gameObject.SetActive(_isMobile);
        }
    }

    private void FixedUpdate()
    {
        // Вся физика в FixedUpdate для стабильности
        if (_isMobile && _joystick != null && Time.timeScale != 0f)
        {
            Joy();
        }
        else
        {
            Keyboard();
        }
    }

    private void Update()
    {
        // Логика поворота и событий
        float currentHorizontal = _isMobile && _joystick != null
            ? _joystick.Horizontal
            : Input.GetAxisRaw(NameDirectionHorizontal);

        // Поворот персонажа с мертвой зоной
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

        // Вызываем событие Run только при движении
        if (_rigidbody2D.linearVelocity.sqrMagnitude > 0.01f)
        {
            Run?.Invoke();
        }
    }

    public void Initialize(DynamicJoystick joystick)
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
}
