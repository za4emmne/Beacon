using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(CharactersHealth))]

public class EnemyAttacked : MonoBehaviour
{
    [SerializeField] private CharactersHealth _charactersHealth;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _damage = 1.5f;
    [SerializeField] private float _startTimeAttack = 0.8f;
    [SerializeField] private int _delay = 2;

    public event Action AnimationAttackPlayed;

    private float _attackTime = 0;

    private void Start()
    {
        _charactersHealth = GetComponent<CharactersHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            player.GetComponent<CharactersHealth>().TakeDamage(_damage);
            AnimationAttackPlayed?.Invoke();
        }
    }
}