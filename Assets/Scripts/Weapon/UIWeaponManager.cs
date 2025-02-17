using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponManager : MonoBehaviour
{
    [Header("Скрипты:")]
    [SerializeField] private ManagerWeapon _manager;

    [Header("Объекты:")]
    [SerializeField] private GameObject _weaponPanel;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Text[] _texts;
    [SerializeField] private Image[] _icons;

    private int _playerLevel;
    [SerializeField] private PlayerProgress _progress;

    private void Awake()
    {
        //_progress = Player.singleton.GetComponent<PlayerProgress>();
    }

    private void Start()
    {
        OnDissactivePanel();
    }

    private void OnEnable()
    {
        _progress.LevelUp += ShowPanel;
    }

    private void OnDisable()
    {
        _progress.LevelUp -= ShowPanel;
    }

    public void OnDissactivePanel()
    {
        _weaponPanel.SetActive(false);
    }

    private void ShowPanel()
    {
        List<WeaponData> choices = _manager.GetRandomChoices();//позже добавить уровень

        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i < choices.Count)
            {
                _buttons[i].gameObject.SetActive(true);
                _texts[i].text = choices[i].name;
                _icons[i].sprite = choices[i].icon;
                int index = 1;//?
                _buttons[i].onClick.RemoveAllListeners(); //удаляет событие клика по кнопке, которое было установлено в предыдущей части кода.
                _buttons[i].onClick.AddListener(() => OnChoiceSelected(choices[index]));
            }
            else
            {
                _buttons[i].gameObject.SetActive(false);
            }
        }

        FreezeTime(0f);
        _weaponPanel.SetActive(true); //сделать анимацию
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
