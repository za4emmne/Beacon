using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using static Cinemachine.DocumentationSortingAttribute;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private ButtonManager _buttonManager;
    [SerializeField] private Text _playerTalk;
    [SerializeField] private Button _achive;
    [SerializeField] private Button _start;
    [SerializeField] private Button _statsButton;
    [SerializeField] private RectTransform _statsPanel;
    [SerializeField] private Button _closeStatsPanel;

    [Header("Настройки анимации")]
    [SerializeField] private Vector2 _hiddenPosition;
    [SerializeField] private Vector2 _shownPosition;
    [SerializeField] private float _animationDuration;
    [SerializeField] private UIStats _stats;

    private int _bestScoreKill = -1;
    private int _bestLevel = -1;

    private void Awake()
    {
        _statsPanel.anchoredPosition = _hiddenPosition;
        _statsPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        _achive.onClick.AddListener(ShowAchiveScreen);
        _start.onClick.AddListener(StartGame);
        _statsButton.onClick.AddListener(ShowStats);
        _closeStatsPanel.onClick.AddListener(HideStats);
    }

    private void OnEnable()
    {
        _buttonManager.OnShowTooltip += ShowDemo;
        _buttonManager.OnHideTooltip += HideDemo;
        YG2.onGetSDKData += LoadData;
    }

    private void OnDisable()
    {
        _buttonManager.OnShowTooltip -= ShowDemo;
        _buttonManager.OnHideTooltip -= HideDemo;
        YG2.onGetSDKData -= LoadData;
    }

    private void LoadData()
    {
        _bestScoreKill = GameDataManager.Instance.BestScore;
        _bestLevel = GameDataManager.Instance.BestLevel;
    }

    private void HideStats()
    {
        _statsPanel.DOAnchorPos(_hiddenPosition, _animationDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            _statsPanel.gameObject.SetActive(false);
        });
    }

    private void ShowStats()
    {
        _stats.CurrentStatsUpdate(GameDataManager.Instance.BestScore, GameDataManager.Instance.BestLevel, "0");
        _statsPanel.gameObject.SetActive(true);
        _statsPanel.DOAnchorPos(_shownPosition, _animationDuration).SetEase(Ease.OutQuad);
    }

    private void ShowDemo()//сделать случайную фразу
    {

        if (_playerTalk.gameObject.activeSelf != true)
        {
            _playerTalk.gameObject.SetActive(true);
            _playerTalk.text = "Нажимай, Шрек!";
        }
        else
        {
            _playerTalk.text = "Нажимай, Шрек!";
        }
    }

    private void HideDemo()
    {
        if (_playerTalk.gameObject.activeSelf)
            _playerTalk.gameObject.SetActive(false);
    }

    private void ShowAchiveScreen()
    {
        Debug.Log("Show");
    }

    public void StartGame()//добавить подписку на события
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;

        SceneManager.LoadScene("Game");
    }
}
