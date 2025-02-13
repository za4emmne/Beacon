using System;
using UnityEngine;
[CreateAssetMenu(fileName = "new Enemy", menuName = "Enemy/Create new Enemy")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string _name;
    //[SerializeField] private Sprite _sprite;
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _prefab;

    //public Sprite Sprite => _sprite;
    public string Name => _name;
    public float Health => _health;
    public float Damage => _damage;
    public float Speed => _speed;
    public GameObject Prefab => _prefab;
}
