using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
//дописать событие подсказок!
public class UIWeaponManager : MonoBehaviour
{
    [Header("Скрипты:")]
    [SerializeField] private ManagerWeapon _manager;

    [Header("Объекты:")]
    [SerializeField] private GameObject _weaponPanel;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Text[] _texts;
    [SerializeField] private Text[] _levelText;
    [SerializeField] private Image[] _icons;
    [SerializeField] private ParticleSystem _confeti;
    [SerializeField] private Text _levelUp;
    [SerializeField] private PlayerProgress _progress;

    [Header("Настройки анимации")]
    [SerializeField] private Vector2 _hiddenPosition;
    [SerializeField] private Vector2 _shownPosition;
    [SerializeField] private Vector2 _hiddenTooltipPosition;
    [SerializeField] private Vector2 _shownTooltipPosition;
    [SerializeField] private float _animationDuration;

    [Header("Подсказка")]
    [SerializeField] private GameObject _tooltipPanel;
    [SerializeField] private Text _tooltipText; 

    private RectTransform _weaponPanalRect;
    private RectTransform _tooltipPanelRect;
    private ButtonManager[] _buttonManagers;
    private WeaponData[] _currentChoices;

    private void Awake()
    {
        _weaponPanalRect = _weaponPanel.GetComponent<RectTransform>();
        _tooltipPanelRect = _tooltipPanel.GetComponent<RectTransform>();
        _buttonManagers = new ButtonManager[_buttons.Length];
        //_tooltipPanel.SetActive(false);
    }

    private void Start()
    {
        _weaponPanalRect.anchoredPosition = _hiddenPosition;
        _weaponPanel.SetActive(false);
        _levelUp.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _progress.LevelUp += ConfetiBoom;
    }

    private void OnDisable()
    {
        _progress.LevelUp -= ConfetiBoom;
    }

    public void OnDissactivePanel()
    {
        _weaponPanalRect.DOAnchorPos(_hiddenPosition, _animationDuration);
        _weaponPanel.SetActive(false);
    }

    private void ConfetiBoom()
    {
        _confeti.Play();
        LevelUpAnimationText();
    }

    private void LevelUpAnimationText()
    {
        Vector3 levelUpTransform = _levelUp.transform.position;
        _levelUp.text = "Level UP";
        _levelUp.gameObject.SetActive(true);
        _levelUp.transform.DOMoveY(_levelUp.transform.position.y + 8, 2)
            .SetEase(Ease.OutQuad);
        _levelUp.DOFade(0, 2).OnComplete(() =>
        {
            _levelUp.transform.position = levelUpTransform;
            _levelUp.gameObject.SetActive(false);
            _levelUp.DOFade(1, 0).OnComplete(() =>
            {
                ShowPanel();
            });
        });
    }

    private void ShowPanel()
    {
        PlayerProgress player = Player.singleton.GetComponent<PlayerProgress>();
        _currentChoices = _manager.GetRandomChoices().ToArray(); // Сохраняем текущие варианты

        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i < _currentChoices.Length)
            {
                _buttons[i].gameObject.SetActive(true);
                _texts[i].text = _currentChoices[i].Name;
                _icons[i].sprite = _currentChoices[i].Icon;

                if (_currentChoices[i].CurrentLevel > 0)
                    _levelText[i].text = "lvl: " + _currentChoices[i].CurrentLevel.ToString();
                else
                    _levelText[i].text = "New!";

                // Инициализация менеджера кнопки
                if (_buttonManagers[i] == null)
                {
                    _buttonManagers[i] = _buttons[i].gameObject.GetComponent<ButtonManager>();
                    int index = i; // Важно сохранить копию индекса для замыкания

                    _buttonManagers[i].OnShowTooltip += () => ShowTooltip(index);
                    _buttonManagers[i].OnHideTooltip += HideTooltip;
                }

                WeaponData currentCoice = _currentChoices[i];
                _buttons[i].onClick.RemoveAllListeners();
                _buttons[i].onClick.AddListener(() => OnChoiceSelected(currentCoice));
            }
            else
            {
                _buttons[i].gameObject.SetActive(false);
            }
        }

        _weaponPanel.SetActive(true);
        _weaponPanalRect.DOAnchorPos(_shownPosition, _animationDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            FreezeTime(0f);
        });
    }


    private void ShowTooltip(int buttonIndex)
    {
        if (buttonIndex >= 0 && buttonIndex < _currentChoices.Length)
        {
            _tooltipText.text = _currentChoices[buttonIndex].Description;
            _tooltipPanel.SetActive(true);
            _tooltipPanelRect.DOKill(); // Останавливаем предыдущие анимации
            _tooltipPanelRect.DOAnchorPos(_shownTooltipPosition, _animationDuration).SetEase(Ease.OutQuad)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }
    }

    private void HideTooltip()
    {
        _tooltipPanelRect.DOKill(); // Останавливаем предыдущие анимации
        _tooltipPanelRect.DOAnchorPos(_hiddenTooltipPosition, _animationDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => _tooltipPanel.SetActive(false))
            .SetUpdate(true); // Анимация будет работать даже при Time.timeScale = 0
    }

    private void OnChoiceSelected(WeaponData selectedWeaponAbility)
    {
        _manager.OnWeaponAbilitySelected(selectedWeaponAbility);
        OnDissactivePanel();
        FreezeTime(1f);
    }

    private void FreezeTime(float freeze)
    {
        Time.timeScale = freeze;
    }
}
