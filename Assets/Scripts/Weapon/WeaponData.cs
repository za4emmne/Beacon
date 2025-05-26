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
    [SerializeField] public string Name;
    [SerializeField] public string Description;
    [SerializeField] public Sprite Icon;
    [SerializeField] public GameObject Prefab;
    [SerializeField] public float Damage;
    [SerializeField] public float delay;
    [SerializeField] public float attackRange;
    [SerializeField] public float speed;
    [SerializeField] public int level;
 

    private void Start()
    {
        level = 0;
    }
}
