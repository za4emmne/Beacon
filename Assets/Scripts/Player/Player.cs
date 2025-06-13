using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CameraShake _camera;
    [SerializeField] private float _maxHealth;

    private PlayerAnimation _animator;
    private PlayerHealth _health;
    private PlayerMovement _movenment;

    public static Player singleton {  get; private set; }

    private void Awake()
    {
        _movenment = GetComponent<PlayerMovement>();
        _health = GetComponent<PlayerHealth>();
        _animator = GetComponent<PlayerAnimation>();
        singleton = this;
    }

    private void Start()
    {
        _health.Init(_maxHealth);
    }

    public void Initialize(UIManager uiManager, CameraShake camera, FixedJoystick joystick)
    {
        _animator.Initialize(uiManager);
        _camera = camera;
        _movenment.Initialize(joystick);
    }

    public void TakeDamage(float damage)
    {
        _camera.Shake();
        _animator.OnGetDamageAnimation();
        _health.TakeDamage(damage);
    }
}
