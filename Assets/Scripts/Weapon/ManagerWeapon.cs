using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerWeapon : MonoBehaviour
{
    [SerializeField] private List<WeaponData> _weapons;

    private GeneratorWeapon _generator;

    private int _numberOfChoices = 3;
    private List<WeaponData> _availableWeapons;

    private void Awake()
    {
        _generator = GetComponent<GeneratorWeapon>();
    }

    public List<WeaponData> GetRandomChoices()//добавить разное оружие в зависимости от уровня
    {
        _availableWeapons = new List<WeaponData>(_weapons);
        List<WeaponData> choices = new List<WeaponData>();

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
        if (selectedWeaponAbility.Prefab != null)
        {
            selectedWeaponAbility.level++;

            if (selectedWeaponAbility.weaponType == TypeWeapon.Melee)
            {
                _generator.GetObject();
            }
            else if (selectedWeaponAbility.weaponType == TypeWeapon.Ranged)
            {
                _generator.OnStartGenerator();
            }
        }
        else
        {
            Debug.LogWarning("Prefab is missing for: " + selectedWeaponAbility.name);
        }
    }
}
