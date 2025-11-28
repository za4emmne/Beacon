using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using System;
using Cinemachine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Компоненты игрока")]
    [SerializeField] private GameObject _player;
    [SerializeField] private CameraShake _camera;
    [SerializeField] private FixedJoystick _fixedJoystick;
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

    public int BestLevel => GameDataManager.Instance.BestLevel;
    public int HighScore => GameDataManager.Instance.BestScore;
    public int CurrentKill => _kill;
    public int CurrentCoin => _currentCoin;
    public int Level => _level;
    public int RaiseCount => _raiseCount;
    public bool IsAddPrise => _isAddPrise;

    public event Action PlayerRaist;
    public event Action<int> OnAddCoin;

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _weaponWeapon = GetComponent<ManagerWeapon>();
        _uiManager = GetComponent<UIManager>();

        CreatePlayer();

        if (!_initialized)
        {
            _initialized = true;
            StartCoroutine(InitializeGame());
        }
    }

    private void Start()
    {
        _raiseCount = 1;
        _kill = 0;
        _isAddPrise = false;
    }

    private void OnEnable()
    {
        _enemyManager.OneKill += ChangeScore;
    }

    private void OnDisable()
    {
        _enemyManager.OneKill -= ChangeScore;
        _progress.LevelUp -= _uiManager.ChangeLevel;
        _progress.LevelUp -= LevelUpAudioPlay;
    }

    public void OnRaisePlayer()
    {
        YG2.RewardedAdvShow(_rewardID, () =>
        {
            if (_raiseCount > 0)
                _raiseCount--;

            _playerHealth.Raise();
            _playerHealth.StartUndeadProcess();
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

    private IEnumerator InitializeGame()
    {
        yield return null;
        //инициализация игрока
        _gameDataManager = GameDataManager.Instance;
        _follower.Playertransform(Player.singleton.transform);
        _progress = Player.singleton.GetComponent<PlayerLevelManager>();
        _playerHealth = Player.singleton.GetComponent<PlayerHealth>();

        _smoothHealthBar.Init(_playerHealth);

        //инициализация камеры
        _сinemachineVirtualCamera.Follow = Player.singleton.transform;

        //инициализация системы врагов
        _waveSystem.Initialized(Player.singleton.transform);
        _waveSystem.StartWave();

        //инициализация 
        _progressBar.Init();
        _uiManager.Init(_progress);
        _weaponWeapon.Init();
        _pillsGenerator.Init(Player.singleton.HillEffect, _playerHealth);
        TilemapChunkManager.Instance.Init();

        _progress.LevelUp += _uiManager.ChangeLevel;
        _progress.LevelUp += LevelUpAudioPlay;
    }

    private void CreatePlayer()
    {
        GameObject player = Instantiate(_player);
        player.GetComponent<Player>().Initialize(_camera, _fixedJoystick);
        player.transform.position = Vector3.zero;
    }
}
