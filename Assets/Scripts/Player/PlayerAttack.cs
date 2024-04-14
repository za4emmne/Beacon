using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _damage = 10;
    [SerializeField] private CharactersHealth _charactersHealth;

    public event Action Attacked;

    public float Damage => _damage;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attacked?.Invoke();
            Attack();
        }
    }

    private void Attack()
    {
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

        if (hitEnemy.Length != 0)
        {
            hitEnemy[0].GetComponent<CharactersHealth>().TakeDamage(_damage);
        }
    }
}