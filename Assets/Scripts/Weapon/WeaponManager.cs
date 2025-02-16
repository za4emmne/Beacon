using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<WeaponData> _weapons;
    [SerializeField] private WeaponGenerator _generator;

    private int _numberOfChoices = 3;
    private List<WeaponData> _availableWeapons;

    private void Start()
    {
        _availableWeapons = new List<WeaponData>(_weapons);
    }

    public List<WeaponData> GetRandomChoices()
    {
        List<WeaponData> choices = new List<WeaponData>();

        for(int i = 0; i < _numberOfChoices; i++)
        {
            int randomIndex = Random.Range(0, _availableWeapons.Count);
            choices.Add(_availableWeapons[randomIndex]);
            _availableWeapons.RemoveAt(randomIndex);
        }

        return choices;
    }

    public void OnWeaponAbilitySelected(WeaponData selectedWeaponAbility)
    {
        // Удаляем выбранное оружие или способность из списка доступных
        //availableWeaponsAbilities.Remove(selectedWeaponAbility);

        // Создаем префаб выбранного оружия или способности
        if (selectedWeaponAbility.Prefab != null)
        {
            _generator.GetObject();
            //Instantiate(selectedWeaponAbility.prefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Prefab is missing for: " + selectedWeaponAbility.name);
        }
    }
}
