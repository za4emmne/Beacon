using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerWeapon : MonoBehaviour
{
    [SerializeField] private List<WeaponData> _allWeapons;

    //[SerializeField] private EnemiesGenerator _enemyGenerators;
    private Transform _playerWeaponPoint;
    private PlayerWeapons _player;
    private PlayerLevelManager _playerProgress;
    private List<WeaponController> _weaponsInHand;
    private List<WeaponData> _availableWeapons;
    private int _numberOfChoices = 3;

    public void Init()
    {
        _player = Player.singleton.GetComponent<PlayerWeapons>();
        _playerProgress = Player.singleton.GetComponent<PlayerLevelManager>();
        _playerWeaponPoint = _player.Point;

        foreach (var weapon in _allWeapons)
        {
            weapon.CurrentLevel = 0;
        }

        CreateNewWeapon(_player.GetStartWeapon());
    }

    public List<WeaponData> GetRandomChoices()
    {
        List<WeaponData> choices = new List<WeaponData>();
        _availableWeapons = new();

        foreach (var weapon in _allWeapons)
        {
            if (weapon.LevelOpen <= _playerProgress.Level)
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
                selectedWeaponAbility.CurrentLevel++;
                weaponController.Initialize(selectedWeaponAbility);
            }
        }
    }

    private void CreateNewWeapon(WeaponData weaponData)
    {
        Debug.Log("Создаем оружие - " + weaponData.name);
        var weaponPrefab = Instantiate(weaponData.Prefab, _playerWeaponPoint.position,
            _playerWeaponPoint.rotation, _playerWeaponPoint);

        WeaponController weaponController = weaponPrefab.GetComponent<WeaponController>();
        GeneratorWeapon weaponGenerator = weaponPrefab.GetComponent<GeneratorWeapon>();

        if (weaponController == null)
        {
            Debug.LogError($"WeaponController отсутствует на префабе {weaponData.name}");
            return;
        }

        weaponData.GetStartLevel();
        weaponData.Init();
        weaponController.Initialize(weaponData);
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
