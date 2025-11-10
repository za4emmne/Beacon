using UnityEngine;

public enum TypeWeapon
{
    Melee,
    Ranged,
    Upgraid
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon")]

public class WeaponData : ScriptableObject
{
    [SerializeField] public TypeWeapon weaponType;
    [SerializeField] public string Name;
    [SerializeField] public string Description;
    [SerializeField] public Sprite Icon;
    [SerializeField] public GameObject Prefab;
    [SerializeField] private float _damage;
    [SerializeField] private float _delay;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _speed;
    [SerializeField] private int _level;
    [SerializeField] public int LevelOpen;
    [SerializeField] public WeaponController Controller;

    [Header("Текущие значения")]
    public float CurrentDamage;
    public float CurrentDelay;
    public float CurrentAttackRange;
    public float CurrentSpeed;
    public int CurrentLevel;
    public string CurrentDescription;
 

    public void Init()
    {
        CurrentDamage = _damage;
        CurrentDelay = _delay;
        CurrentAttackRange = _attackRange;
        CurrentSpeed = _speed;
        CurrentLevel = _level;
        //CurrentDescription = Controller.Description;
    }

    public void GetStartLevel()
    {
        _level = 1;
    }
}
