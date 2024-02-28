using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    private int _damage = 10;
    public float Damage => _damage;
    public UnityEvent AnimationAttack;

    private void Start()
    {
    }

    private void Update()
    {
        Attack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy) && Attack())
        {
            Debug.Log("Attack");
            enemy.TakeDamage(_damage);
        }
    }

    private bool Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimationAttack ?.Invoke();
            return true;
        }
        else
            return false;
    }
}
