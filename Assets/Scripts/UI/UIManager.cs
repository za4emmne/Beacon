using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    [Header("Скрипты")]
    [SerializeField] private PlayerProgress _player;
    [Header("UI элементы")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _levelText;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _settingButton;

    private Button _pause;

    private ButtonManager _settingButtonManager;
    private PauseMenuManager _pauseMenuManager;

    private void Awake()
    {
        _settingButtonManager = _settingButton.GetComponent<ButtonManager>();
        _pauseMenuManager = GetComponent<PauseMenuManager>();
        _pause = _settingButton.GetComponent<Button>();
    }

    private void Start()
    {
        _gameOverScreen.SetActive(false);
        ChangeLevel();
        _pause.onClick.AddListener(_pauseMenuManager.Pause);
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

    public void ChangeScore(int Score)
    {
        _scoreText.text = "Счет: " + Score;
    }

    public void ChangeLevel()
    {
        _levelText.text = "Уровень: " + _player.Level;
    }

    public void OnDeadScreen()
    {
        _gameOverScreen.SetActive(true);
    }

    private void OnPauseScreen()
    {
        _pauseMenuManager.Pause();
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
