using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using System;
using Cinemachine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static Action OnGameRestart;
    public static bool IsRestarting = false;

    [Header("Компоненты игрока")]
    [SerializeField] private GameObject _player;
    [SerializeField] private CameraShake _camera;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private CinemachineVirtualCamera _сinemachineVirtualCamera;
    [SerializeField] private Follower _follower;
    [SerializeField] private SmoothHealthBar _smoothHealthBar;

    private PlayerLevelManager _progress;
    private PlayerHealth _playerHealth;
    private ManagerWeapon _weaponWeapon;

    [Header("Менеджеры врагов")]
    [SerializeField] private EnemiesGenerator _enemyManager;
    [SerializeField] private WaveSystem _waveSystem;

    [Header("Скрпипты игры")]
    [SerializeField] private StarsSpawner _starGenerator;
    [SerializeField] private PillsGenerator _pillsGenerator;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private AudioClip _levelUpAudio;

    private AudioSource _audioSource;
    private GameDataManager _gameDataManager;
    private UIManager _uiManager;

    private int _kill;
    private int _currentCoin;
    private int _raiseCount;
    private int _level;
    private bool _initialized = false;
    private string _rewardID;
    private int _coinPrice = 100;
    private bool _isAddPrise;

    public int BestLevel => GameDataManager.Instance?.BestLevel ?? 0;
    public int HighScore => GameDataManager.Instance?.BestScore ?? 0;
    public int CurrentKill => _kill;
    public int CurrentCoin => _currentCoin;
    public int Level => _level;
    public int RaiseCount => _raiseCount;
    public bool IsAddPrise => _isAddPrise;

    public event Action PlayerRaist;
    public event Action<int> OnAddCoin;

    private void Awake()
    {
        InitDebug.Log($"[INIT][GameManager] Awake() called - Instance={Instance != null}, this={GetInstanceID()}, scene={SceneManager.GetActiveScene().name}");
        
        if (Instance != null && Instance != this)
        {
            InitDebug.LogWarning($"[INIT][GameManager] Duplicate detected, destroying this");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _weaponWeapon = GetComponent<ManagerWeapon>();
        _uiManager = GetComponent<UIManager>();
        
        InitDebug.Log("[INIT][GameManager] Resetting game state");
        _initialized = false;
        _kill = 0;
        _raiseCount = 1;
        _isAddPrise = false;
    }

    private void Start()
    {
        InitDebug.Log($"[INIT][GameManager] Start() called - initialized={_initialized}");
        StartCoroutine(InitializeRoutine());
    }

    private IEnumerator InitializeRoutine()
    {
        InitDebug.Log("[INIT][GameManager] InitializeRoutine started");
        
        while (GameDataManager.Instance == null)
        {
            InitDebug.Log("[INIT][GameManager] Waiting for GameDataManager...");
            yield return null;
        }
        
        InitDebug.Log("[INIT][GameManager] GameDataManager ready, calling CreatePlayer");
        CreatePlayer();

        if (!_initialized)
        {
            _initialized = true;
            InitDebug.Log("[INIT][GameManager] Starting InitializeGame coroutine");
            StartCoroutine(InitializeGame());
        }
    }

    private void OnEnable()
    {
        InitDebug.Log($"[INIT][GameManager] OnEnable() - _enemyManager={_enemyManager != null}");
        
        if (_enemyManager != null)
        {
            _enemyManager.OneKill += ChangeScore;
            InitDebug.Log("[EVENT][GameManager] Subscribed to OneKill");
        }
    }

    private void OnDisable()
    {
        InitDebug.Log("[INIT][GameManager] OnDisable()");
        
        if (_enemyManager != null)
        {
            _enemyManager.OneKill -= ChangeScore;
            InitDebug.Log("[EVENT][GameManager] Unsubscribed from OneKill");
        }
        
        if (_progress != null)
        {
            if (_uiManager != null)
                _progress.LevelUp -= _uiManager.ChangeLevel;
            _progress.LevelUp -= LevelUpAudioPlay;
            InitDebug.Log("[EVENT][GameManager] Unsubscribed from LevelUp");
        }
    }

    private void OnDestroy()
    {
        InitDebug.Log($"[INIT][GameManager] OnDestroy() - Instance={Instance?.GetInstanceID()}, this={GetInstanceID()}");
        
        if (Instance == this)
        {
            Instance = null;
            InitDebug.Log("[INIT][GameManager] Instance set to null");
        }
    }

    private IEnumerator InitializeGame()
    {
        InitDebug.Log("[INIT][GameManager] InitializeGame started");
        yield return null;

        _gameDataManager = GameDataManager.Instance;
        yield return null;

        if (Player.singleton == null)
        {
            InitDebug.LogError("[INIT][GameManager] Player.singleton is NULL!");
            yield break;
        }

        InitDebug.Log($"[INIT][GameManager] Setting up Player - singleton={Player.singleton.GetInstanceID()}");
        
        _follower.Playertransform(Player.singleton.transform);
        _progress = Player.singleton.GetComponent<PlayerLevelManager>();
        yield return null;

        _playerHealth = Player.singleton.GetComponent<PlayerHealth>();
        _smoothHealthBar.Init(_playerHealth);
        yield return null;

        _сinemachineVirtualCamera.Follow = Player.singleton.transform;
        yield return null;

        InitDebug.Log("[INIT][GameManager] Initializing WaveSystem and EnemiesGenerator");
        _waveSystem.Initialized(Player.singleton.transform);
        _enemyManager.SetPlayerTransform(Player.singleton.transform);
        yield return null;

        _waveSystem.StartWave();
        yield return null;

        _progressBar.Init();
        _uiManager.Init(_progress);
        _weaponWeapon.Init();
        yield return null;

        _pillsGenerator.Init(Player.singleton.HillEffect, _playerHealth);
        
        BiomeData currentLocation = _gameDataManager.CurrentLocation;
        TilemapChunkManager.Instance.SetLocation(currentLocation);
        TilemapChunkManager.Instance.Init();
        yield return null;

        _progress.LevelUp += _uiManager.ChangeLevel;
        _progress.LevelUp += LevelUpAudioPlay;
        
        InitDebug.Log("[EVENT][GameManager] Subscribed to LevelUp events");
        InitDebug.Log("[INIT][GameManager] InitializeGame COMPLETE");
    }

    private void CreatePlayer()
    {
        CharacterData character = null;
        
        if (GameDataManager.Instance != null)
        {
            character = GameDataManager.Instance.CurrentCharacter;
        }
        
        if (character == null || character.playerPrefab == null)
        {
            InitDebug.LogWarning("[INIT][GameManager] Character not selected, using default");
            if (GameDataManager.Instance != null && GameDataManager.Instance.Characters != null)
            {
                character = GameDataManager.Instance.Characters.Find(c => c.isDefault);
            }
            if (character == null || character.playerPrefab == null)
            {
                character = _player.GetComponent<Player>()?.GetComponent<CharacterData>();
                if (character == null)
                {
                    InitDebug.LogError("[INIT][GameManager] FAILED TO CREATE PLAYER!");
                    return;
                }
            }
        }

        GameObject prefabToSpawn = character.playerPrefab;
        GameObject player = Instantiate(prefabToSpawn);
        
        InitDebug.Log($"[INIT][GameManager] Player created - instanceID={player.GetInstanceID()}, position={player.transform.position}");
        
        player.GetComponent<Player>().Initialize(_camera, _joystick);
        
        var playerWeapons = player.GetComponent<PlayerWeapons>();
        if (character.startedWeapon != null)
            playerWeapons.AddStartWeapon(character.startedWeapon);
        
        player.transform.position = Vector3.zero;
        InitDebug.Log("[INIT][GameManager] Player Initialize complete");
    }

    public void OnRaisePlayer()
    {
        YG2.RewardedAdvShow(_rewardID, () =>
        {
            if (_raiseCount > 0)
                _raiseCount--;

            _playerHealth.StartUndeadProcess();
            Player.singleton.GetComponent<PlayerAnimation>().OnRecoverAnimation();
            Player.singleton.UndeadEffect.Play();
            _uiManager.UndeadTextActivate();
            PlayerRaist?.Invoke();
        });

        Time.timeScale = 1f;
    }

    public void OnGetCoins()
    {
        YG2.RewardedAdvShow(_rewardID, () =>
        {
            AddCoins(_coinPrice);
        });
    }

    public void AddCoins(int amount)
    {
        _currentCoin += amount;
        OnAddCoin?.Invoke(_currentCoin);
        GameDataManager.Instance.AddCoins(amount);
    }

    private void ChangeScore()
    {
        _kill++;
        _uiManager.ChangeScore(_kill);
        _gameDataManager.UpdateBestScore(_kill);
    }

    private void LevelUpAudioPlay()
    {
        _audioSource.PlayOneShot(_levelUpAudio);
    }
}