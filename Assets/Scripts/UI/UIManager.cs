using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _levelText;
    [SerializeField] private GameObject[] _gameOverScreen;
    [SerializeField] private PlayerProgress _player;
    [SerializeField] private GameObject _settingButton;

    private ButtonManager _settingButtonManager;

    private void Awake()
    {
        _settingButtonManager = _settingButton.GetComponent<ButtonManager>();
    }

    private void Start()
    {
        ScreenManage(_gameOverScreen, false);
    }

    private void OnEnable()
    {
        _settingButtonManager.OnShowTooltip += ShowSettingButtonAnimation;
    }

    private void OnDisable()
    {
        _settingButtonManager.OnShowTooltip -= ShowSettingButtonAnimation;
    }

    public void ChangeScore(int Score)
    {
        _scoreText.text = "—чет: " + Score;
    }

    public void ChangeLevel()
    {
        _levelText.text = "”ровень: " + _player.Level;
    }

    public void OnDeadScreen()
    {
        ScreenManage(_gameOverScreen, true);
    }

    private void ScreenManage(GameObject[] gameObjects, bool status)
    {
        foreach (var ui in gameObjects)
        {
            ui.SetActive(status);
        }
    }

    private void ShowSettingButtonAnimation() //доделать
    {
        _settingButton.transform.DORotate(new Vector3(0, 0, 180), 0.5f)
            .SetEase(Ease.InCirc);
    }
}
