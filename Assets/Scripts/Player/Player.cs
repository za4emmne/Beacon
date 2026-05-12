using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        InitDebug.Log($"[INIT][Player] Awake() - singleton={singleton?.GetInstanceID()}, this={GetInstanceID()}, scene={SceneManager.GetActiveScene().name}");
        
        if (singleton != null && singleton != this)
        {
            InitDebug.LogWarning($"[INIT][Player] Duplicate! destroying this={GetInstanceID()}, keep singleton={singleton.GetInstanceID()}");
            Destroy(gameObject);
            return;
        }
        
        _movenment = GetComponent<PlayerMovement>();
        _health = GetComponent<PlayerHealth>();
        _animator = GetComponent<PlayerAnimation>();
        singleton = this;
        
        InitDebug.Log($"[INIT][Player] singleton set to {singleton.GetInstanceID()}");
    }

    private void Start()
    {
        InitDebug.Log($"[INIT][Player] Start() - maxHealth={_maxHealth}, current={_health?.Current}");
        _health?.Init(_maxHealth);
    }

    private void OnDestroy()
    {
        InitDebug.Log($"[INIT][Player] OnDestroy() - singleton={singleton?.GetInstanceID()}, this={GetInstanceID()}");
        
        if (singleton == this)
        {
            singleton = null;
            InitDebug.Log("[INIT][Player] singleton = null");
        }
    }

    public void Initialize(CameraShake camera, Joystick joystick)
    {
        InitDebug.Log("[INIT][Player] Initialize() called");
        _animator?.Initialize();
        _camera = camera;
        _movenment?.Initialize(joystick);
        InitDebug.Log("[INIT][Player] Initialize() complete");
    }

    public void TakeDamage(float damage, Vector2 hitSourcePosition)
    {
        if (_health?.Current > 0)
        {
            _camera?.Shake();
            _health?.TakeDamage(damage, hitSourcePosition);
        }
    }
}