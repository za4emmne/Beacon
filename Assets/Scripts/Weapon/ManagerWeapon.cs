using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerWeapon : MonoBehaviour
{
    [SerializeField] private Transform _playerWeaponoint;
    [SerializeField] private List<WeaponData> _allWeapons;
    [SerializeField] private ObjectKiller _killer;

    private PlayerWeapons _player;
    private List<WeaponController> _weaponsInHand;
    private List<WeaponData> _availableWeapons;
    private int _numberOfChoices = 3;

    private void Awake()
    {
        _player = Player.singleton.GetComponent<PlayerWeapons>();
    }

    private void Start()
    {
        foreach (var weapon in _allWeapons)
        {
            weapon.level = 0;
        }
    }

    public List<WeaponData> GetRandomChoices()//добавить разное оружие в зависимости от уровня
    {
        List<WeaponData> choices = new List<WeaponData>();
        _availableWeapons = new();

        foreach (var weapon in _allWeapons)
        {
            _availableWeapons.Add(weapon);
        }

        for (int i = 0; i < _numberOfChoices; i++)
        {
            int randomIndex = Random.Range(0, _availableWeapons.Count);
            choices.Add(_availableWeapons[randomIndex]);
            _availableWeapons.RemoveAt(randomIndex);
        }

        return choices;
    }

    public void OnWeaponAbilitySelected(WeaponData selectedWeaponAbility)
    {
        _weaponsInHand = new List<WeaponController>(_player.Weapons());

        foreach (var weapon in _weaponsInHand)
        {
            Debug.Log("Текущее оружие - " + weapon.name);
        }

        if (selectedWeaponAbility.Prefab == null)//проверка на наличие префаба
        {
            Debug.LogWarning("Prefab is missing for: " + selectedWeaponAbility.name);
            return;
        }
        else
        {
            WeaponController weaponController = FindWeaponInHand(selectedWeaponAbility);

            if (weaponController == null)
            {
                CreateNewWeapon(selectedWeaponAbility);
            }
            else
            {
                Debug.Log("Улучшаем оружие - " + selectedWeaponAbility.name);
                selectedWeaponAbility.level++;
                weaponController.Initialize(selectedWeaponAbility);
            }
        }
    }

    private void CreateNewWeapon(WeaponData weaponData)
    {
        Debug.Log("Создаем оружие - " + weaponData.name);
        var weaponPrefab = Instantiate(weaponData.Prefab, _playerWeaponoint.position,
            _playerWeaponoint.rotation, _playerWeaponoint);

        WeaponController weaponController = weaponPrefab.GetComponent<WeaponController>();

        if (weaponController == null)
        {
            Debug.LogError($"WeaponController отсутствует на префабе {weaponData.name}");
            return;
        }

        weaponData.level = 1;
        weaponController.Initialize(weaponData); //дописать в корне 
        _player.AddNewWeapon(weaponController);
    }

    private WeaponController FindWeaponInHand(WeaponData weaponData)
    {
        foreach (var weapon in _player.Weapons())
        {
            if (weapon.data == weaponData)
                return weapon;
        }

        return null;
    }
}
