using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG.LanguageLegacy;
using YG;
using UnityEditor;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private PlayerLevelManager _player;
    private UIWeaponManager _uiWeaponManager;
    private ButtonManager _settingButtonManager;
    private PauseMenuManager _pauseMenuManager;
    private UIGameOverManager _gameOverManager;
    private PlayerWeapons _playerWeapon;
    private GameManager _gameManager;

    [Header("UI элементы")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _coinText;
    [SerializeField] private GameObject _settingButton;
    [SerializeField] private Button _restart;
    [SerializeField] private Image[] _icons;
    [SerializeField] private Button _menu;
    [SerializeField] private Text _undeadText;

    private Button _pause;

    private void Awake()
    {
        InitDebug.Log($"[INIT][UIManager] Awake() - Instance={Instance?.GetInstanceID()}, this={GetInstanceID()}, scene={SceneManager.GetActiveScene().name}");
        
        if (Instance != null && Instance != this)
        {
            InitDebug.LogWarning("[INIT][UIManager] Duplicate! Destroying this");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        _gameOverManager = GetComponent<UIGameOverManager>();
        _uiWeaponManager = GetComponent<UIWeaponManager>();
        _settingButtonManager = _settingButton.GetComponent<ButtonManager>();
        _pauseMenuManager = GetComponent<PauseMenuManager>();
        _pause = _settingButton.GetComponent<Button>();
        _gameManager = GameManager.Instance;
        
        InitDebug.Log("[INIT][UIManager] Components initialized");
    }

    private void Start()
    {
        InitDebug.Log("[INIT][UIManager] Start()");
        
        _gameOverManager?.OnDeadScreenDisactivate();
        _pause?.onClick?.AddListener(() => _pauseMenuManager?.Pause());
        _restart?.onClick?.AddListener(RestartScene);
        _menu?.onClick?.AddListener(LoadMenuScene);
        
        if (_undeadText != null)
            _undeadText.gameObject.SetActive(false);
            
        InitDebug.Log("[INIT][UIManager] Button listeners registered");
    }

    private void OnEnable()
    {
        InitDebug.Log("[INIT][UIManager] OnEnable()");
        
        if (_gameManager != null)
        {
            _gameManager.OnAddCoin += ChangeCoin;
            InitDebug.Log("[EVENT][UIManager] Subscribed to OnAddCoin");
        }
        
        if (_settingButtonManager != null)
        {
            _settingButtonManager.OnShowTooltip += ShowSettingButtonAnimation;
            _settingButtonManager.OnHideTooltip += ShowSettingButtonAnimationExit;
            InitDebug.Log("[EVENT][UIManager] Subscribed to Tooltip events");
        }
        
        if (_gameManager != null)
        {
            _gameManager.PlayerRaist += () => _gameOverManager?.OnDeadScreenDisactivate();
            InitDebug.Log("[EVENT][UIManager] Subscribed to PlayerRaist");
        }
    }

    private void OnDisable()
    {
        InitDebug.Log("[INIT][UIManager] OnDisable()");
        
        if (_gameManager != null)
        {
            _gameManager.OnAddCoin -= ChangeCoin;
            InitDebug.Log("[EVENT][UIManager] Unsubscribed from OnAddCoin");
        }
        
        if (_settingButtonManager != null)
        {
            _settingButtonManager.OnShowTooltip -= ShowSettingButtonAnimation;
            _settingButtonManager.OnHideTooltip -= ShowSettingButtonAnimationExit;
            InitDebug.Log("[EVENT][UIManager] Unsubscribed from Tooltip events");
        }
        
        if (_uiWeaponManager != null)
        {
            _uiWeaponManager.WeaponIsChoise -= AddIcon;
            InitDebug.Log("[EVENT][UIManager] Unsubscribed from WeaponIsChoise");
        }
        
        if (_gameManager != null)
        {
            _gameManager.PlayerRaist -= () => _gameOverManager?.OnDeadScreenDisactivate();
            InitDebug.Log("[EVENT][UIManager] Unsubscribed from PlayerRaist");
        }
    }

    private void OnDestroy()
    {
        InitDebug.Log($"[INIT][UIManager] OnDestroy() - Instance={Instance?.GetInstanceID()}, this={GetInstanceID()}");
        
        if (Instance == this)
            Instance = null;
    }

    public void Init(PlayerLevelManager player)
    {
        InitDebug.Log("[INIT][UIManager] Init() called");
        
        if (Player.singleton == null)
        {
            InitDebug.LogError("[INIT][UIManager] Player.singleton is NULL in Init()!");
            return;
        }
        
        _playerWeapon = Player.singleton.GetComponent<PlayerWeapons>();
        _player = Player.singleton.GetComponent<PlayerLevelManager>();
        
        _uiWeaponManager?.Init();
        ChangeLevel();
        
        if (_playerWeapon?.StartWeapon != null)
            AddIcon(_playerWeapon.StartWeapon);
        
        if (_uiWeaponManager != null)
            _uiWeaponManager.WeaponIsChoise += AddIcon;
        
        InitDebug.Log("[INIT][UIManager] Init() complete");
    }

    public void UndeadTextActivate()
    {
        if (_undeadText == null) return;
        
        Vector3 levelUpTransform = _undeadText.transform.position;
        _undeadText.gameObject.SetActive(true);
        _undeadText.transform.DOMoveY(_undeadText.transform.position.y + 10, 3)
            .SetEase(Ease.OutQuad);
        _undeadText.DOFade(0, 3).OnComplete(() =>
        {
            _undeadText.transform.position = levelUpTransform;
            _undeadText.gameObject.SetActive(false);
            _undeadText.DOFade(1, 0);
        });
    }

    public void OnDeadScreenActivate()
    {
        InitDebug.Log("[UI][UIManager] OnDeadScreenActivate() called");
        _gameOverManager?.OnDeadScreenActivate();
    }

    public void LoadMenuScene()
    {
        InitDebug.Log("[RESTART][UIManager] LoadMenuScene() called");
        
        if (GameDataManager.Instance != null && _gameManager != null)
        {
            GameDataManager.Instance.UpdateTotalKill(_gameManager.CurrentKill);
            GameDataManager.Instance.UpdateTotalTime(Timer.Instance?.GetCurrentTime() ?? 0);
            GameDataManager.Instance.UpdateBestTime(Timer.Instance?.GetCurrentTime() ?? 0);
        }
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ChangeScore(int Score)
    {
        if (_scoreText == null) return;
        
        _scoreText.text = LocalizationManager.Instance.GetTranslation("kill_text")
                               .Replace("{killCount}", Score.ToString());
    }

    public void ChangeLevel()
    {
        if (_levelText == null || _player == null) return;
        
        _levelText.text = LocalizationManager.Instance.GetTranslation("level_text")
                        .Replace("{level}", _player.Level.ToString());
    }

    public void ChangeCoin(int value)
    {
        if (_coinText == null) return;
        
        _coinText.text = LocalizationManager.Instance.GetTranslation("coin_text")
                        .Replace("{countCoins}", value.ToString());
    }

    public void SetSettingButton(bool set)
    {
        if (_settingButton != null)
            _settingButton.gameObject.SetActive(set);
    }

    private void AddIcon(WeaponData weapon)
    {
        if (_icons != null && _icons.Length > 0 && weapon != null)
            _icons[0].sprite = weapon.Icon;
    }

private void RestartScene()
    {
        InitDebug.Log("[RESTART][UIManager] RestartScene() called");
        
        Time.timeScale = 1f;
        
        if (GameDataManager.Instance != null)
            GameDataManager.Instance.ResetRunData();
        
        InitDebug.Log("[RESTART] Loading scene 'Game'");
        SceneManager.LoadScene("Game");
    }

    private void ShowSettingButtonAnimation()
    {
        if (_settingButton != null)
            _settingButton.transform.DORotate(new Vector3(0, 0, 180), 0.5f)
                .SetEase(Ease.InCirc);
    }

    private void ShowSettingButtonAnimationExit()
    {
        if (_settingButton != null)
            _settingButton.transform.DORotate(new Vector3(0, 0, 360), 0.5f)
                .SetEase(Ease.InCirc);
    }
}