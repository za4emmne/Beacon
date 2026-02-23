using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerWeapon : MonoBehaviour
{
    [SerializeField] private List<WeaponData> _allWeapons;
    [SerializeField] private int _maxSlots = 5;

    //[SerializeField] private EnemiesGenerator _enemyGenerators;
    private Transform _playerWeaponPoint;
    private PlayerWeapons _player;
    private PlayerLevelManager _playerProgress;
    private List<WeaponController> _weaponsInHand;
    private List<WeaponData> _availableWeapons;
    private int _numberOfChoices = 3;

    public int MaxSlots => _maxSlots;

    public void Init()
    {
        _player = Player.singleton.GetComponent<PlayerWeapons>();
        _playerProgress = Player.singleton.GetComponent<PlayerLevelManager>();
        _playerWeaponPoint = _player.Point;

        foreach (var weapon in _allWeapons)
        {
            weapon.CurrentLevel = 0;
        }

        var startWeaponData = _player.StartWeapon;

        // 1. Создаём стартовое оружие
        CreateNewWeapon(startWeaponData);

        // 2. ИСПОЛНЯЕМ улучшение сразу на только что созданном
        WeaponController createdController = _player.Weapons().Last(); // последнее добавленное
        createdController.Initialize(startWeaponData);

        // 3. Опционально: если хочешь сразу уровень 2
        startWeaponData.CurrentLevel++; // с 1 → 2
        createdController.Initialize(startWeaponData);
    }

    public List<WeaponData> GetRandomChoices()
    {
        List<WeaponData> choices = new List<WeaponData>();
        choices.Add(null);
        choices.Add(null);
        choices.Add(null);

        int currentSlotsUsed = GetUsedSlotsCount();
        bool hasFreeSlots = currentSlotsUsed < _maxSlots;

        List<WeaponData> ownedWeapons = new List<WeaponData>();
        List<WeaponData> newWeapons = new List<WeaponData>();
        List<WeaponData> passiveUpgrades = new List<WeaponData>();
        List<WeaponData> ownedPassives = new List<WeaponData>();

        foreach (var weapon in _allWeapons)
        {
            if (weapon.LevelOpen > _playerProgress.Level)
                continue;

            if (weapon.weaponType == TypeWeapon.Upgraid)
            {
                if (IsWeaponOwned(weapon))
                {
                    if (weapon.CurrentLevel < 4)
                        ownedPassives.Add(weapon);
                }
                else if (weapon.CurrentLevel < 4)
                {
                    passiveUpgrades.Add(weapon);
                }
                continue;
            }

            if (IsWeaponOwned(weapon))
            {
                if (weapon.CurrentLevel < 4)
                    ownedWeapons.Add(weapon);
            }
            else if (weapon.CurrentLevel < 4)
            {
                newWeapons.Add(weapon);
            }
        }

        // Слот 1: Оружие игрока (улучшение) или новое (если есть слоты)
        if (ownedWeapons.Count > 0)
        {
            choices[0] = ownedWeapons[Random.Range(0, ownedWeapons.Count)];
        }
        else if (hasFreeSlots && newWeapons.Count > 0)
        {
            choices[0] = newWeapons[Random.Range(0, newWeapons.Count)];
        }

        // Слот 2: Новое оружие (если есть слоты) или улучшение оружия
        if (hasFreeSlots && newWeapons.Count > 0)
        {
            choices[1] = newWeapons[Random.Range(0, newWeapons.Count)];
        }
        else if (ownedWeapons.Count > 0)
        {
            choices[1] = ownedWeapons[Random.Range(0, ownedWeapons.Count)];
        }

        // Слот 3: Пассивка (новая или улучшение)
        List<WeaponData> allPassives = new List<WeaponData>();
        allPassives.AddRange(ownedPassives);
        allPassives.AddRange(passiveUpgrades);

        if (allPassives.Count > 0)
        {
            choices[2] = allPassives[Random.Range(0, allPassives.Count)];
        }

        return choices;
    }

    public int GetUsedSlotsCount()
    {
        if (_player == null)
            _player = Player.singleton.GetComponent<PlayerWeapons>();

        return _player.Weapons() != null ? _player.Weapons().Count : 0;
    }

    public bool HasFreeSlots()
    {
        return GetUsedSlotsCount() < _maxSlots;
    }

    private bool IsWeaponOwned(WeaponData weapon)
    {
        foreach (var w in _player.Weapons())
        {
            if (w.data == weapon)
                return true;
        }
        return false;
    }

    public void OnWeaponAbilitySelected(WeaponData selectedWeaponAbility)
    {
        if (selectedWeaponAbility == null)
        {
            Debug.LogWarning("Selected weapon ability is null!");
            return;
        }

        _weaponsInHand = new List<WeaponController>(_player.Weapons());

        foreach (var weapon in _weaponsInHand)
        {
            Debug.Log("Текущее оружие - " + weapon.name);
        }

        // Пассивки не имеют Prefab - обрабатываем отдельно
        if (selectedWeaponAbility.weaponType == TypeWeapon.Upgraid)
        {
            WeaponController existingController = FindUpgradeInHand(selectedWeaponAbility);
            if (existingController != null)
            {
                Debug.Log("Улучшаем пассивку - " + selectedWeaponAbility.name);
                selectedWeaponAbility.CurrentLevel++;
                existingController.Initialize(selectedWeaponAbility);
            }
            else
            {
                CreateNewWeapon(selectedWeaponAbility);
            }
            return;
        }

        if (selectedWeaponAbility.Prefab == null)
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

    private WeaponController FindUpgradeInHand(WeaponData weaponData)
    {
        foreach (var w in _player.Weapons())
        {
            if (w.data == weaponData)
                return w;
        }
        return null;
    }

    private void CreateNewWeapon(WeaponData weaponData)
    {
        int currentSlots = GetUsedSlotsCount();
        bool isUpgrade = IsWeaponOwned(weaponData) || (weaponData.weaponType == TypeWeapon.Upgraid && FindUpgradeInHand(weaponData) != null);

        // Проверяем, можем ли мы добавить новое оружие/пассивку
        if (!isUpgrade && currentSlots >= _maxSlots)
        {
            Debug.LogWarning($"Cannot add new weapon. Max slots ({_maxSlots}) reached!");
            return;
        }

        // Пассивки не создают GameObject - только добавляют компонент
        if (weaponData.weaponType == TypeWeapon.Upgraid)
        {
            Debug.Log("Добавляем пассивку - " + weaponData.name);
            if (weaponData.Controller != null)
            {
                GameObject passiveObj = new GameObject(weaponData.name);
                passiveObj.transform.SetParent(_playerWeaponPoint);
                passiveObj.transform.localPosition = Vector3.zero;

                WeaponController controller = passiveObj.AddComponent(weaponData.Controller.GetType()) as WeaponController;
                controller.Initialize(weaponData);
                _player.AddNewWeapon(controller);
            }
            return;
        }

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
