using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Leshy", menuName = "Enemy/Create new Leshy")]
public class LeshyData : EnemyData
{
    [Header("Leshy Settings")]
    [Tooltip("How many bats to spawn per attack")]
    [SerializeField] private int _batsPerAttack = 3;
    
    [Tooltip("Delay between bat spawns in seconds")]
    [SerializeField] private float _batSpawnDelay = 0.5f;
    
    [Tooltip("Reference to bat prefab to spawn")]
    [SerializeField] private GameObject _batPrefab;
    
    [Tooltip("Attack cooldown in seconds")]
    [SerializeField] private float _attackCooldown = 3f;
    
    public int BatsPerAttack => _batsPerAttack;
    public float BatSpawnDelay => _batSpawnDelay;
    public GameObject BatPrefab => _batPrefab;
    public float AttackCooldown => _attackCooldown;
}