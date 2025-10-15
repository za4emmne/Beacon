using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("�������")]
    [SerializeField] private PlayerLevelManager _player;
    private GameManager _gameManager;
    [Header("UI ��������")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _levelText;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _settingButton;
    [SerializeField] private GameObject _scoreIcon;
    [SerializeField] private Button _restart;
    [SerializeField] private Button _raisePlayer;
    [SerializeField] private Button _menu;

    private Button _pause;

    private ButtonManager _settingButtonManager;
    private PauseMenuManager _pauseMenuManager;

    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
        _settingButtonManager = _settingButton.GetComponent<ButtonManager>();
        _pauseMenuManager = GetComponent<PauseMenuManager>();
        _pause = _settingButton.GetComponent<Button>();
    }

    private void Start()
    {

        OnDeadScreenDisactivate();
        ChangeLevel();
        _pause.onClick.AddListener(_pauseMenuManager.Pause);
        _restart.onClick.AddListener(RestartScene);
        _raisePlayer.onClick.AddListener(_gameManager.OnRaisePlayer);
        _menu.onClick.AddListener(LoadMenuScene);
    }

    private void OnEnable()
    {
        _settingButtonManager.OnShowTooltip += ShowSettingButtonAnimation;
        _settingButtonManager.OnHideTooltip += ShowSettingButtonAnimationExit;
    }

    private void OnDisable()
    {
        _settingButtonManager.OnShowTooltip += ShowSettingButtonAnimation;
        _settingButtonManager.OnHideTooltip -= ShowSettingButtonAnimationExit;
    }

    public void LoadMenuScene()
    {
        Debug.Log("Loasd");
        SceneManager.LoadScene("Menu");
    }

    public void ChangeScore(int Score)
    {
        _scoreText.text = "����: " + Score;
    }

    public void ChangeLevel()
    {
        _levelText.text = "�������: " + _player.Level;
    }

    public void OnDeadScreenActivate()
    {
        _gameOverScreen.SetActive(true);
        _scoreIcon.SetActive(false);

        if(_gameManager.RaiseCount <= 0)
            _raisePlayer.gameObject.SetActive(false);
    }

    public void OnDeadScreenDisactivate()
    {
        _gameOverScreen.SetActive(false);
        _scoreIcon.SetActive(true);
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
