using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeWeapon
{
    Melee,
    Ranged
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon")]

public class WeaponData : ScriptableObject
{
    [SerializeField] public TypeWeapon weaponType;
    [SerializeField] public float damage;
    [SerializeField] public float delay;
    [SerializeField] public float attackRange;
    [SerializeField] public GameObject Prefab;
    [SerializeField] public float speed;
    [SerializeField] public Sprite icon;
}
