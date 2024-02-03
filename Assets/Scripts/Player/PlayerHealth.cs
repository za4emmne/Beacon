using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _health;

    public int _maxHealth = 100;

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

    public int GetMaxHealth
    {
        get { return _maxHealth; }
    }

    public int GetHealth
    {
        get {return _health; }        
    }
}
