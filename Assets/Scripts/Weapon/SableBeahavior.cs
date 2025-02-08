using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SableBeahavior : MonoBehaviour
{
    [Header("Attack Stats")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _delay;

    private void Update()
    {
        DrawAttackArea(100, Color.red);
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);
        CharactersHealth enemyHealth;

        if (hitEnemies.Length != 0)
        {
            foreach (var enemy in hitEnemies)
            {
                int random = Random.Range(0, 2);

                if (random == 0)
                {
                    enemyHealth = enemy.GetComponent<CharactersHealth>();
                    enemyHealth.TakeDamage(_damage);
                }
            }
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
}
