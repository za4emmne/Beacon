using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private List<WeaponController> _weapons;
    [SerializeField] private Transform _point;
    [SerializeField] private WeaponData _startWeapon;

    private PlayerProgress _progress;

    private void Awake()
    {
        _progress = GetComponent<PlayerProgress>();
    }

    public List<WeaponController> Weapons()
    {
        return _weapons;
    }

    public void AddNewWeapon(WeaponController weapon)
    {
        _weapons.Add(weapon);   
    }

    public WeaponData GetStartWeapon()
    {
        return _startWeapon;
    }
}
