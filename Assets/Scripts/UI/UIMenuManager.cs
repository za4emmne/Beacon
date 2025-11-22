using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using YG;
using YG.LanguageLegacy;
using static Cinemachine.DocumentationSortingAttribute;

public class UIMenuManager : MonoBehaviour
{
    //[SerializeField] private ButtonManager _buttonManager;
    [SerializeField] private Text _playerTalk;
    [SerializeField] private Button _achive;
    [SerializeField] private Button _start;


    [Header("Панель статистики")]
    [SerializeField] private Button _statsButton;
    [SerializeField] private RectTransform _statsPanel;
    [SerializeField] private Button _closeStatsPanel;
    [SerializeField] private Text _totalKillText;
    [SerializeField] private Text _totalTimeText;

    [Header("Магазин")]
    [SerializeField] private Button _shopButton;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private Button _closeShopPanel;

    [Header("Настройки анимации")]
    [SerializeField] private Vector2 _hiddenPosition;
    [SerializeField] private Vector2 _shownPosition;
    [SerializeField] private float _animationDuration;
    [SerializeField] private UIStats _stats;

    private LangYGAdditionalText _additionalScoreText;
    private LangYGAdditionalText _additionalTimeText;
    private int _bestScoreKill = -1;
    private int _bestLevel = -1;

    private void Awake()
    {
        _additionalScoreText = _totalKillText.gameObject.GetComponent<LangYGAdditionalText>();
        _additionalTimeText = _totalTimeText.gameObject.GetComponent<LangYGAdditionalText>();
        _statsPanel.anchoredPosition = _hiddenPosition;
        _statsPanel.gameObject.SetActive(false);
        HideShopPanel();
    }

    private void Start()
    {
        _achive.onClick.AddListener(ShowAchiveScreen);
        _start.onClick.AddListener(StartGame);
        _statsButton.onClick.AddListener(ShowStats);
        _closeStatsPanel.onClick.AddListener(HideStats);
        _shopButton.onClick.AddListener(ShowShopPanel);
        _closeShopPanel.onClick.AddListener(HideShopPanel);
    }

    private void OnEnable()
    {
        //_buttonManager.OnShowTooltip += ShowDemo;
        //_buttonManager.OnHideTooltip += HideDemo;
        YG2.onGetSDKData += LoadData;
    }

    private void OnDisable()
    {
        //_buttonManager.OnShowTooltip -= ShowDemo;
        //_buttonManager.OnHideTooltip -= HideDemo;
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
        _stats.CurrentStatsUpdate(GameDataManager.Instance.TotalKill,
            GameDataManager.Instance.BestLevel, GetTimeText(GameDataManager.Instance.BestTime), GameDataManager.Instance.TotalCoins);

        _additionalScoreText.additionalText = GameDataManager.Instance.BestScore.ToString();
        _additionalTimeText.additionalText = GetTimeText(GameDataManager.Instance.TotalTime);

        _statsPanel.gameObject.SetActive(true);
        _statsPanel.DOAnchorPos(_shownPosition, _animationDuration).SetEase(Ease.OutQuad);
    }

    private string GetTimeText(float time)
    {
        string timeMin = LocalizationManager.Instance.GetTranslation("min_text");
        string timeSec = LocalizationManager.Instance.GetTranslation("sec_text");
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);

        return string.Format("{0:00 " + timeMin + "} {1:00 " + timeSec + "}", minutes, seconds);
    }

    //private void ShowDemo()//сделать случайную фразу
    //{

    //    if (_playerTalk.gameObject.activeSelf != true)
    //    {
    //        _playerTalk.gameObject.SetActive(true);
    //        _playerTalk.text = "Нажимай, Шрек!";
    //    }
    //    else
    //    {
    //        _playerTalk.text = "Нажимай, Шрек!";
    //    }
    //}

    //private void HideDemo()
    //{
    //    if (_playerTalk.gameObject.activeSelf)
    //        _playerTalk.gameObject.SetActive(false);
    //}

    private void ShowAchiveScreen()
    {
        Debug.Log("Show");
    }

    private void ShowShopPanel()
    {
        _shopPanel.SetActive(true);
    }

    private void HideShopPanel()
    {
        _shopPanel.gameObject.SetActive(false);
    }

    public void StartGame()//добавить подписку на события
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;

        SceneManager.LoadScene("Game");
    }
}
