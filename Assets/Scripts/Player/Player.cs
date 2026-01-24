using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerWeapons))]
[RequireComponent(typeof(PlayerLevelManager))]

public class Player : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private ParticleSystem _hillEffect;
    [SerializeField] private ParticleSystem _levelUpEffect;
    [SerializeField] private ParticleSystem _undeadEffect;

    private CameraShake _camera;
    private PlayerAnimation _animator;
    private PlayerHealth _health;
    private PlayerMovement _movenment;

    public ParticleSystem LevelUpEffect => _levelUpEffect;
    public ParticleSystem HillEffect => _hillEffect;
    public ParticleSystem UndeadEffect => _undeadEffect;
    public static Player singleton { get; private set; }

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

    public void Initialize(CameraShake camera, FixedJoystick joystick)
    {
        _animator.Initialize();
        _camera = camera;
        _movenment.Initialize(joystick);

    }

    public void TakeDamage(float damage, Vector2 hitSourcePosition)
    {
        if (_health.Current > 0)
        {
            _camera.Shake();
            _health.TakeDamage(damage, hitSourcePosition);
        }
    }
}
