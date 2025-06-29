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
    private PlayerProgress _playerProgress;
    private List<WeaponController> _weaponsInHand;
    private List<WeaponData> _availableWeapons;
    private int _numberOfChoices = 3;

    private void Awake()
    {
        _player = Player.singleton.GetComponent<PlayerWeapons>();
        _playerProgress = Player.singleton.GetComponent<PlayerProgress>();
    }

    private void Start()
    {
        foreach (var weapon in _allWeapons)
        {
            weapon.CurrentLevel = 0;
        }

        CreateNewWeapon(_player.GetStartWeapon());
    }

    public List<WeaponData> GetRandomChoices()//�������� ������ ������ � ����������� �� ������
    {
        List<WeaponData> choices = new List<WeaponData>();
        _availableWeapons = new();

        foreach (var weapon in _allWeapons)
        {
            _availableWeapons.Add(weapon);
        }

        foreach (var weapon in _availableWeapons)
        {
            if(weapon.LevelOpen > _playerProgress.Level)
                _availableWeapons.Remove(weapon);
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
            Debug.Log("������� ������ - " + weapon.name);
        }

        if (selectedWeaponAbility.Prefab == null)//�������� �� ������� �������
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
                Debug.Log("�������� ������ - " + selectedWeaponAbility.name);
                selectedWeaponAbility.CurrentLevel++;
                weaponController.Initialize(selectedWeaponAbility);
            }
        }
    }

    private void CreateNewWeapon(WeaponData weaponData)
    {
        Debug.Log("������� ������ - " + weaponData.name);
        var weaponPrefab = Instantiate(weaponData.Prefab, _playerWeaponoint.position,
            _playerWeaponoint.rotation, _playerWeaponoint);

        WeaponController weaponController = weaponPrefab.GetComponent<WeaponController>();

        if (weaponController == null)
        {
            Debug.LogError($"WeaponController ����������� �� ������� {weaponData.name}");
            return;
        }

        weaponData.CurrentLevel = 1;
        weaponData.Init();
        weaponController.Initialize(weaponData); //�������� � ����� 
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
