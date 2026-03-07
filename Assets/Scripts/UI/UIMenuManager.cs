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
    [SerializeField] private Button _start;


    [Header("ПК элементы")]
    [SerializeField] private GameObject _pcMenuPanel;
    [SerializeField] private Button _statsButton;
    [SerializeField] private RectTransform _statsPanel;
    [SerializeField] private Button _closeStatsPanel;
    [SerializeField] private Text _totalKillText;
    [SerializeField] private Text _totalTimeText;

    [Header("Магазин")]
    [SerializeField] private Button _shopButton;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private Button _closeShopPanel;
    [SerializeField] private GameObject _mainShopPanel;

    [Header("Кнопки магазина")]
    [SerializeField] private Button _heroesShopButton;
    [SerializeField] private Button _achiveShopButton;
    [SerializeField] private Button _locationShopButton;

    [Header("Магазин героев")]
    [SerializeField] private GameObject _shopHeroesPanel;
    [SerializeField] private Button _closeShopHeroPanel;

    [Header("Магазин ачивок")]
    [SerializeField] private Button _achive;
    [SerializeField] private GameObject _shopAchivePanel;
    [SerializeField] private Button _closeShopAchivePanel;

    [Header("Магазин локаций")]
    [SerializeField] private GameObject _shopLocationsPanel;
    [SerializeField] private Button _closeShopLocationsPanel;
    [SerializeField] private LocationShopUI _locationShopUI;

    [Header("Настройки анимации")]
    [SerializeField] private Vector2 _hiddenPosition;
    [SerializeField] private Vector2 _shownPosition;
    [SerializeField] private float _animationDuration;
    [SerializeField] private UIStats _stats;

    [Header("Мобильная версия")]
    [SerializeField] private GameObject _mobileBackgroundImage;

    [Header("Мобильный магазин")]
    [SerializeField] private GameObject _mobileShopPanel;
    [SerializeField] private Button _mobileShopTabButton;
    [SerializeField] private Image _mobileShopTabIcon;
    [SerializeField] private Button _mobilePrevTabButton;
    [SerializeField] private Button _mobileNextTabButton;
    [SerializeField] private Sprite _heroesIcon;
    [SerializeField] private Sprite _achiveIcon;

    [Header("Ежедневные награды")]
    [SerializeField] private Button _dailyRewardButton;
    [SerializeField] private Text _dailyRewardTimerText;

    [Header("Мобильные панели магазина")]
    [SerializeField] private GameObject _mobileHeroesPanel;
    [SerializeField] private GameObject _mobileAchivePanel;

    private int _currentMobileShopTab = 0;
    private readonly int _totalMobileTabs = 2;

    private CharacterShop _shop;
    private LangYGAdditionalText _additionalScoreText;
    private LangYGAdditionalText _additionalTimeText;
    private int _bestScoreKill = -1;
    private int _bestLevel = -1;
    private bool _isMobile;

    private void Awake()
    {
        _shop = GetComponent<CharacterShop>();
        _additionalScoreText = _totalKillText.gameObject.GetComponent<LangYGAdditionalText>();
        _additionalTimeText = _totalTimeText.gameObject.GetComponent<LangYGAdditionalText>();
        _statsPanel.anchoredPosition = _hiddenPosition;
        _statsPanel.gameObject.SetActive(false);

        HideShopPanel();
        HideAchiveScreen();
        HideHeroesShopPanel();
        HideLocationsShopPanel();
    }

    private void Start()
    {
        _isMobile = DeviceDetector.Instance != null ? DeviceDetector.Instance.IsMobile : false;
        Debug.Log($"[UIMenuManager] Start: _isMobile={_isMobile}, DeviceDetector={DeviceDetector.Instance?.IsMobile}");
        
        ConfigureMenuForPlatform();

        //_achive.onClick.AddListener(ShowAchiveScreen);
        _start.onClick.AddListener(StartGame); //старт игры (1 кнопка)
        _statsButton.onClick.AddListener(ShowStats); //статистика (2 кнопка)
        _closeStatsPanel.onClick.AddListener(HideStats); //закрыть панель статистики
        _shopButton.onClick.AddListener(ShowShopPanel); //магазин(3 кнопка)
        _closeShopPanel.onClick.AddListener(HideShopPanel); //закрыть магазин(нажатие кнопки)
        //кнопки магазина
        _heroesShopButton.onClick.AddListener(ShowHeroesShopPanel);
        _closeShopHeroPanel.onClick.AddListener(HideHeroesShopPanel);
        _closeShopAchivePanel.onClick.AddListener(HideAchiveScreen);

        _achiveShopButton.onClick.AddListener(ShowAchiveScreen);

        // Магазин локаций
        if (_locationShopButton != null)
            _locationShopButton.onClick.AddListener(ShowLocationsShopPanel);
        if (_closeShopLocationsPanel != null)
            _closeShopLocationsPanel.onClick.AddListener(HideLocationsShopPanel);

        // Ежедневная награда
        if (_dailyRewardButton != null)
            _dailyRewardButton.onClick.AddListener(OnDailyRewardButtonClick);

        // Мобильный магазин
        if (_isMobile)
        {
            if (_mobileShopTabButton != null)
                _mobileShopTabButton.onClick.AddListener(OnMobileShopTabClick);
            if (_mobilePrevTabButton != null)
                _mobilePrevTabButton.onClick.AddListener(PreviousMobileShopTab);
            if (_mobileNextTabButton != null)
                _mobileNextTabButton.onClick.AddListener(NextMobileShopTab);
            UpdateMobileShopTab();
        }
    }

    private void OnMobileShopTabClick()
    {
        if (_currentMobileShopTab == 0 && _mobileHeroesPanel != null)
        {
            ShowHeroesShopPanel();
        }
        else if (_currentMobileShopTab == 1 && _mobileAchivePanel != null)
        {
            ShowAchiveScreen();
        }
    }

    private void PreviousMobileShopTab()
    {
        _currentMobileShopTab = (_currentMobileShopTab - 1 + _totalMobileTabs) % _totalMobileTabs;
        UpdateMobileShopTab();
    }

    private void NextMobileShopTab()
    {
        _currentMobileShopTab = (_currentMobileShopTab + 1) % _totalMobileTabs;
        UpdateMobileShopTab();
    }

    private void UpdateMobileShopTab()
    {
        if (_mobileShopTabIcon != null)
        {
            _mobileShopTabIcon.sprite = _currentMobileShopTab == 0 ? _heroesIcon : _achiveIcon;
        }

        if (_mobileHeroesPanel != null)
            _mobileHeroesPanel.SetActive(_currentMobileShopTab == 0);
        if (_mobileAchivePanel != null)
            _mobileAchivePanel.SetActive(_currentMobileShopTab == 1);
    }

    private void ConfigureMenuForPlatform()
    {
        Debug.Log($"[UIMenuManager] ConfigureMenuForPlatform: _isMobile={_isMobile}, _mobileBackgroundImage={_mobileBackgroundImage}, _pcMenuPanel={_pcMenuPanel}");
        
        if (_isMobile)
        {

            if (_pcMenuPanel != null)
                _pcMenuPanel.SetActive(false);
            if (_mobileBackgroundImage != null)
                _mobileBackgroundImage.SetActive(true);
            if (_mobileShopPanel != null)
                _mobileShopPanel.SetActive(true);
        }
        else
        {
            if (_pcMenuPanel != null)
                _pcMenuPanel.SetActive(true);
            if (_mobileBackgroundImage != null)
                _mobileBackgroundImage.SetActive(false);
            if (_mobileShopPanel != null)
                _mobileShopPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        YG2.onGetSDKData += LoadData;
    }

    private void OnDisable()
    {
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
            GameDataManager.Instance.BestLevel, GetTimeText(GameDataManager.Instance.BestTime),
            GameDataManager.Instance.TotalCoins);

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

    private void ManageObject(GameObject panel, bool status)
    {
        panel.SetActive(status);
    }

    private void HideMainShopPanel()
    {
        _mainShopPanel.SetActive(false);
    }

    private void ShowMainShopPanel()
    {
        _mainShopPanel.SetActive(true);
    }

    private void ShowHeroesShopPanel()
    {
        HideMainShopPanel();

        _shopHeroesPanel.gameObject.SetActive(true);
        _shopHeroesPanel.transform.DOLocalMoveY(10, 1f);
        _closeShopHeroPanel.gameObject.SetActive(true);
    }

    private void HideHeroesShopPanel()
    {
        _shopHeroesPanel.gameObject.SetActive(false);
        _closeShopHeroPanel.gameObject.SetActive(false);

        ShowMainShopPanel();
    }

    private void ShowAchiveScreen()
    {
        _shopAchivePanel.SetActive(true);
    }

    private void HideAchiveScreen()
    {
        _shopAchivePanel.SetActive(false);
    }

    private void ShowShopPanel()
    {
        HideAchiveScreen();
        HideHeroesShopPanel();

        if (_isMobile && _mobileShopPanel != null)
        {
            _mobileShopPanel.SetActive(true);
            UpdateMobileShopTab();
        }
        else
        {
            _shopPanel.SetActive(true);
        }
    }

    private void HideShopPanel()
    {
        _shopPanel.gameObject.SetActive(false);

        if (_mobileShopPanel != null)
            _mobileShopPanel.SetActive(false);
    }

    private void ShowLocationsShopPanel()
    {
        Debug.Log("[UIMenuManager] ShowLocationsShopPanel called");
        
        HideHeroesShopPanel();
        HideAchiveScreen();
        HideMainShopPanel();

        if (_shopLocationsPanel != null)
        {
            Debug.Log("[UIMenuManager] _shopLocationsPanel found, activating");
            _shopLocationsPanel.SetActive(true);

            if (_locationShopUI != null)
            {
                Debug.Log("[UIMenuManager] Calling _locationShopUI.Show()");
                _locationShopUI.Show();
            }
            else
            {
                Debug.LogError("[UIMenuManager] _locationShopUI is NULL!");
            }
        }
        else
        {
            Debug.LogError("[UIMenuManager] _shopLocationsPanel is NULL!");
        }
    }

    private void HideLocationsShopPanel()
    {
        if (_shopLocationsPanel != null)
            _shopLocationsPanel.SetActive(false);

        ShowMainShopPanel();
    }

    private void OnDailyRewardButtonClick()
    {
        if (_shop != null)
        {
            _shop.OnDailyRewardButtonClick();
        }
    }

    private void ShowHeroShopBeforeBegin()
    {
        ShowShopPanel();
        ShowHeroesShopPanel();
        _closeShopHeroPanel.gameObject.SetActive(false);
    }

    public void StartGame()//�������� �������� �� �������
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;

        ShowHeroShopBeforeBegin();

        //
    }

    public void StartAfter()
    {
        SceneManager.LoadScene("Game");
    }
}
