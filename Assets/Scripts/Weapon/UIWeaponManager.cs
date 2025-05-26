using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private float _animationDuration;

    private int _playerLevel;
    private RectTransform _weaponPanalRect;

    private void Awake()
    {
        _weaponPanalRect = _weaponPanel.GetComponent<RectTransform>();
        //_manager = GetComponent<ManagerWeapon>();   
        //_progress = Player.singleton.GetComponent<PlayerProgress>();
    }

    private void Start()
    {
        _weaponPanalRect.anchoredPosition = _hiddenPosition;
        _weaponPanel.SetActive(false);
        _levelUp.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //_progress.LevelUp += ShowPanel;
        _progress.LevelUp += ConfetiBoom;
    }

    private void OnDisable()
    {
        // _progress.LevelUp -= ShowPanel;
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
        List<WeaponData> choices = _manager.GetRandomChoices();//позже добавить уровень

        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i < choices.Count)
            {
                _buttons[i].gameObject.SetActive(true);
                _texts[i].text = choices[i].Name;
                _icons[i].sprite = choices[i].Icon;

                if (choices[i].level > 0)
                    _levelText[i].text = "lvl: " + choices[i].level.ToString();
                else
                    _levelText[i].text = "New!";

                WeaponData currentCoice = choices[i];
                _buttons[i].onClick.RemoveAllListeners(); //удаляет событие клика по кнопке, которое было установлено в предыдущей части кода.
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
