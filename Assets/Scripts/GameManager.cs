using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using System;
using Cinemachine;
using System.Collections;

public class GameManager : MonoBehaviour
{
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
    [SerializeField] private StarGenerator _starGenerator;
    [SerializeField] private PillsGenerator _pillsGenerator;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private AudioClip _levelUpAudio;

    private UIManager _uiManager;
    private AudioSource _audioSource;

    private int _kill;
    private int _highScore;
    private int _coins;
    private int _raiseCount;
    private int _level;
    private bool _initialized = false;
    private string _rewardID;

    public int Kill => _kill;
    public int HighScore => _highScore;
    public int Coins => _coins;
    public int Level => _level;
    public int RaiseCount => _raiseCount;

    public event Action PlayerRaist;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GetComponent<UIManager>();
        _weaponWeapon = GetComponent<ManagerWeapon>();
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
    }

    private void OnEnable()
    {
        YG2.onGetSDKData += GetLoad;
        _enemyManager.OneKill += ChangeScore;

    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= GetLoad;
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
            PlayerRaist?.Invoke();
        });

        Time.timeScale = 1f;
    }

    private void GetLoad()
    {
        _highScore = YG2.saves.bestScore;

        if (_progress != null)
        {
            _progress.GetBestLevel();
        }
        else
        {
            _progress = Player.singleton.GetComponent<PlayerLevelManager>();
            _progress.GetBestLevel();
        }

    }

    private void ChangeScore()
    {
        _kill++;
        _uiManager.ChangeScore(_kill);

        if (_kill >= _highScore)
        {
            _highScore -= _kill;
            YG2.saves.bestScore = _highScore;
            YG2.SaveProgress();
        }
    }

    private void LevelUpAudioPlay()
    {
        _audioSource.PlayOneShot(_levelUpAudio);
    }

    private IEnumerator InitializeGame()
    {
        yield return null;
        //инициализация игрока
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
        _progressBar.Init(_progress);
        _uiManager.Init(_progress);
        _weaponWeapon.Init();
        _pillsGenerator.Init(Player.singleton.HillEffect);
        TilemapChunkManager.Instance.Init();

        _progress.LevelUp += _uiManager.ChangeLevel;
        _progress.LevelUp += LevelUpAudioPlay;
    }

    private void CreatePlayer()
    {
        GameObject player = Instantiate(_player);
        player.GetComponent<Player>().Initialize(_uiManager, _camera, _fixedJoystick);
        player.transform.position = Vector3.zero;

    }
}
