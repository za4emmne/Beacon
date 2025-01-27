using UnityEngine;
using System;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _delay;

    [Header("Вспомогательные скрипты")]
    [SerializeField] private PlayerHealth _charactersHealth;
    [SerializeField] private BulletSpawner _weapon;
    [SerializeField] private EnemyManager _enemyManager;

    public event Action Attacked;

    public float Damage => _damage;


    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Attacked?.Invoke();
        //    Attack();
        //}
    }

   

    private IEnumerator Shoot()
    {
        var wait = new WaitForSeconds(_delay);

        while (enabled)
        {

            yield return wait;
        }
    }
}