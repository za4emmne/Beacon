using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float _health;
    private float _maxHealth = 100f;
    public float MaxHealth => _maxHealth;
    public float Health => _health;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void TakeHealth(int pills)
    {
        _health += pills;

        if (_health >= _maxHealth)
            _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
            _health = 0;
    }
}
