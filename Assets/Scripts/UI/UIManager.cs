using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerLevelManager _player;
    private GameManager _gameManager;
    private UIWeaponManager _uiWeaponManager;
    private ButtonManager _settingButtonManager;
    private PauseMenuManager _pauseMenuManager;
    private UIGameOverManager _gameOverManager;
    private PlayerWeapons _playerWeapon;

    [Header("UI элементы")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _levelText;
    [SerializeField] private GameObject _settingButton;
    [SerializeField] private Button _restart;
    [SerializeField] private Image[] _icons;
    [SerializeField] private Button _menu;

    private Button _pause;


    private void Awake()
    {
        _gameOverManager = GetComponent<UIGameOverManager>();
        _uiWeaponManager = GetComponent<UIWeaponManager>();
        _gameManager = GetComponent<GameManager>();
        _settingButtonManager = _settingButton.GetComponent<ButtonManager>();
        _pauseMenuManager = GetComponent<PauseMenuManager>();
        _pause = _settingButton.GetComponent<Button>();
    }

    private void Start()
    {
        _gameOverManager.OnDeadScreenDisactivate();
        _pause.onClick.AddListener(_pauseMenuManager.Pause);
        _restart.onClick.AddListener(RestartScene);
        _menu.onClick.AddListener(LoadMenuScene);
    }

    private void OnEnable()
    {
        _settingButtonManager.OnShowTooltip += ShowSettingButtonAnimation;
        _settingButtonManager.OnHideTooltip += ShowSettingButtonAnimationExit;
        _gameManager.PlayerRaist += _gameOverManager.OnDeadScreenDisactivate;
    }

    private void OnDisable()
    {
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
        AddIcon(_playerWeapon.GetStartWeapon());
        _uiWeaponManager.WeaponIsChoise += AddIcon;
    }

    public void OnDeadScreenActivate()
    {
        _gameOverManager.OnDeadScreenActivate();
    }

    public void LoadMenuScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ChangeScore(int Score)
    {
        _scoreText.text = "—чет: " + Score;
    }

    public void ChangeLevel()
    {
        _levelText.text = "”ровень: " + _player.Level;
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
