using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _damage = 10;
    public float Damage => _damage;
    public event Action AnimationAttackPlayed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimationAttackPlayed?.Invoke();
            Attacked();
        }
    }

    private void Attacked()
    {
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

        if (hitEnemy.Length != 0)
        {
            hitEnemy[0].GetComponent<CharactersHealth>().TakeDamage(_damage);
        }
    }
}
