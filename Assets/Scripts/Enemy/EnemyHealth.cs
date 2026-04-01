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
    [SerializeField] private bool _enableTrailOnHit = true;
    [SerializeField] private float _trailDuration = 2f;
    [SerializeField, Range(0.1f, 2f)] private float _trailIntensity = 0.8f;

    private EnemyMovement _movement;
    private BloodTrailEmitter _bloodTrailEmitter;
    private bool _hasSubscribedToDeath;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _bloodTrailEmitter = GetComponentInChildren<BloodTrailEmitter>();
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

        // Start blood trail on hit if enabled
        if (_enableTrailOnHit && _bloodTrailEmitter != null)
        {
            Debug.Log("Wat?");
            _bloodTrailEmitter.StartBleeding(_trailDuration, _trailIntensity);
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
