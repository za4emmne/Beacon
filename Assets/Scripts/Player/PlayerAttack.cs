using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _damage = 10;
    public float Damage => _damage;
    public UnityEvent AnimationAttack;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimationAttack?.Invoke();
            Attacked();
        }
    }

    private void Attacked()
    {
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

        foreach (var enemy in hitEnemy)
        {
            enemy.GetComponent<CharactersHealth>().TakeDamage(_damage);
        }
    }
}
