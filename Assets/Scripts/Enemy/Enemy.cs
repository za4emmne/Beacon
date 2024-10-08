using System;
using UnityEngine;
[CreateAssetMenu(fileName = "new Enemy", menuName = "Enemy/Create new Enemy")]
public class Enemy : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _health;
    [SerializeField] private float _damage;

    public Sprite Sprite => _sprite;
    public float Health => _health;
    public float Damage => _damage;
}
