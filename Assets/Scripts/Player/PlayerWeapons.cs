using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private List<WeaponController> _weapons;
    [SerializeField] private Transform _point;
    
    [SerializeField] private WeaponData _startWeapon;

    public Transform Point => _point;
    public WeaponData StartWeapon => _startWeapon;

    private void Start()
    {
        
    }

    public List<WeaponController> Weapons()
    {
        return _weapons;
    }

    public void AddNewWeapon(WeaponController weapon)
    {
        _weapons.Add(weapon);   
    }

    public void AddStartWeapon(WeaponData startWeapon)
    {
        _startWeapon = startWeapon;
    }
}
