using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _damage = 10;

    [Header("Вспомогательные скрипты")]
    [SerializeField] private PlayerHealth _charactersHealth;
    [SerializeField] private BulletSpawner _weapon;
    [SerializeField] private EnemyManager _enemyManager;

    public event Action Attacked;

    public float Damage => _damage;


    private void Update()
    {
        DrawAttackArea(_attackRange, Color.red);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attacked?.Invoke();
            Attack();
        }
    }

    private void DrawAttackArea(float pointsCount, Color color)
    {
        List<Vector3> circlePoints = new List<Vector3>();
        float degreesInCircle = 360.0f;
        float angleStep = degreesInCircle / pointsCount * Mathf.Deg2Rad;
        Vector3 center = transform.position;

        for (int i = 0; i < pointsCount; i++)
        {
            float angle = angleStep * i;
            Vector2 point = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _attackRange;
            circlePoints.Add(new Vector3(center.x + point.x, center.y, center.z + point.y));
        }

        for (int i = 0; i < circlePoints.Count - 1; i++)
            Debug.DrawLine(circlePoints[i], circlePoints[i + 1], color);

        Debug.DrawLine(circlePoints[0], circlePoints[circlePoints.Count - 1], color);
    }

    //private IEnumerator Shoot()
    //{
    //    var wait = new WaitForSeconds(_delay);

    //    while (enabled)
    //    {
    //        _weapon.Shoot(_shootPoint);
    //        yield return wait;
    //    }
    //}

    private void Attack()
    {
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);
        CharactersHealth enemy;

        if (hitEnemy.Length != 0)
        {
            enemy = hitEnemy[0].GetComponent<CharactersHealth>();
            enemy.TakeDamage(_damage);
            _enemyManager.CheckHealth(enemy);
        }
    }
}