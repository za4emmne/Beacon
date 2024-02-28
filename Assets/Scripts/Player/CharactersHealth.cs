using UnityEngine;
using UnityEngine.Events;

public class CharactersHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    private float _health;
    private bool _isGameOver;
    public float MaxHealth => _maxHealth;
    public float Health => _health;
    public bool IsGameOver => _isGameOver;

    public UnityEvent DamageMe;
    public UnityEvent AnimationDead;



    private void Awake()
    {
        _health = _maxHealth;
        _isGameOver = false;
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
        {
            _health = 0;
            AnimationDead?.Invoke();
        }

        DamageMe?.Invoke();
    }
}
