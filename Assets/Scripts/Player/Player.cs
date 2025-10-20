using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private ParticleSystem _hillEffect;
    [SerializeField] private ParticleSystem _levelUpEffect;

    private CameraShake _camera;
    private PlayerAnimation _animator;
    private PlayerHealth _health;
    private PlayerMovement _movenment;

    public ParticleSystem LevelUpEffect => _levelUpEffect;
    public ParticleSystem HillEffect => _hillEffect;
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
