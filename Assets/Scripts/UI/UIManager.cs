using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG.LanguageLegacy;
using YG;
using UnityEditor;

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
        Instance = this;
        _gameOverManager = GetComponent<UIGameOverManager>();
        _uiWeaponManager = GetComponent<UIWeaponManager>();
        _settingButtonManager = _settingButton.GetComponent<ButtonManager>();
        _pauseMenuManager = GetComponent<PauseMenuManager>();
        _pause = _settingButton.GetComponent<Button>();
        _gameManager = GetComponent<GameManager>();
    }

    private void Start()
    {
        _gameOverManager.OnDeadScreenDisactivate();
        _pause.onClick.AddListener(_pauseMenuManager.Pause);
        _restart.onClick.AddListener(RestartScene);
        _menu.onClick.AddListener(LoadMenuScene);
        _undeadText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _gameManager.OnAddCoin += ChangeCoin;
        _settingButtonManager.OnShowTooltip += ShowSettingButtonAnimation;
        _settingButtonManager.OnHideTooltip += ShowSettingButtonAnimationExit;
        _gameManager.PlayerRaist += _gameOverManager.OnDeadScreenDisactivate;
    }

    private void OnDisable()
    {
        _gameManager.OnAddCoin -= ChangeCoin;
        _settingButtonManager.OnShowTooltip += ShowSettingButtonAnimation;
        _settingButtonManager.OnHideTooltip -= ShowSettingButtonAnimationExit;
        _uiWeaponManager.WeaponIsChoise -= AddIcon;
        _gameManager.PlayerRaist -= _gameOverManager.OnDeadScreenDisactivate;
    }

    public void Init(PlayerLevelManager player)
    {
        _playerWeapon = Player.singleton.GetComponent<PlayerWeapons>();
        _player = Player.singleton.GetComponent<PlayerLevelManager>();
        _uiWeaponManager.Init();
        ChangeLevel();
        AddIcon(_playerWeapon.StartWeapon);
        _uiWeaponManager.WeaponIsChoise += AddIcon;
    }

    public void UndeadTextActivate()
    {
        Vector3 levelUpTransform = _undeadText.transform.position;
        //_levelUp.text = "Level UP";
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
        _gameOverManager.OnDeadScreenActivate();
    }

    public void LoadMenuScene()
    {
        GameDataManager.Instance.UpdateTotalKill(_gameManager.CurrentKill);
        GameDataManager.Instance.UpdateTotalTime(Timer.Instance.GetCurrentTime());
        GameDataManager.Instance.UpdateBestTime(Timer.Instance.GetCurrentTime());
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ChangeScore(int Score)
    {
        _scoreText.text = LocalizationManager.Instance.GetTranslation("kill_text")
                               .Replace("{killCount}", Score.ToString());
    }

    public void ChangeLevel()
    {
        _levelText.text = LocalizationManager.Instance.GetTranslation("level_text")
                        .Replace("{level}", _player.Level.ToString());
    }

    public void ChangeCoin(int value)
    {
        _coinText.text = LocalizationManager.Instance.GetTranslation("coin_text")
                        .Replace("{countCoins}", value.ToString());
    }

    public void SetSettingButton(bool set)
    {
        _settingButton.gameObject.SetActive(set);
    }

    private void AddIcon(WeaponData weapon)
    {
        _icons[0].sprite = weapon.Icon;
    }

    private void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    private void ShowSettingButtonAnimation()
    {
        _settingButton.transform.DORotate(new Vector3(0, 0, 180), 0.5f)
            .SetEase(Ease.InCirc);
    }

    private void ShowSettingButtonAnimationExit()
    {
        _settingButton.transform.DORotate(new Vector3(0, 0, 360), 0.5f)
            .SetEase(Ease.InCirc);
    }
}
