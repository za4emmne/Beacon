using UnityEngine.Events;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private CharactersHealth _enemyHealth;
    public UnityEvent GetDamage;

    public void TakeDamage(int damage)
    {
        _enemyHealth.TakeDamage(damage);
    }
}
