using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class EnemyHealth : CharactersHealth
{
    [Header("Blood Settings")]
    [SerializeField] private bool _spawnBloodOnHit = true;
    [SerializeField] private bool _spawnBloodOnDeath = true;

    private EnemyMovement _movement;
    private bool _hasSubscribedToDeath;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        if (!_hasSubscribedToDeath)
        {
            Died += OnEnemyDied;
            _hasSubscribedToDeath = true;
        }
    }

    public override void TakeDamage(float damage, Vector2 hitSourcePosition)
    {
        base.TakeDamage(damage, hitSourcePosition);

        GeneratorDamageText.Instance.ShowDamageText(hitSourcePosition, damage);
        _movement.KnockbackFromPlayer(Player.singleton.transform.position);

        if (_spawnBloodOnHit && BloodSplatterManager.Instance != null)
        {
            Vector3 hitPos = new Vector3(hitSourcePosition.x, hitSourcePosition.y, 0);
            Vector3 enemyPos = transform.position;
            Vector3 direction = (enemyPos - hitPos).normalized;
            
            BloodSplatterManager.Instance.SpawnSplatter(hitPos, direction);
        }
    }

    private void OnEnemyDied()
    {
        if (_spawnBloodOnDeath && BloodSplatterManager.Instance != null)
        {
            BloodSplatterManager.Instance.SpawnSplatterAtDeath(transform.position);
        }
    }
}
