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

    private void Update()
    {
        if (_charactersHealth.IsDead == false)
        {
            Attacked();
        }
    }

    private void Attacked()
    {
        if (_attackTime <= 0)
        {
            Collider2D[] heroes = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerLayer);

            if (heroes.Length != 0)
            {
                AnimationAttackPlayed?.Invoke();

                heroes[0].GetComponent<CharactersHealth>().TakeDamage(_damage);
            }

            _attackTime = _startTimeAttack;
        }
        else
        {
            _attackTime -= Time.deltaTime;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.TryGetComponent<Player>(out Player player))
    //    {
    //        InvokeRepeating(nameof(Attacked), 0, 1f);
    //        //_player.GetComponent<PlayerCollisions>().AnimateHit();
    //    }
    //}
}