using UnityEngine;
using UnityEngine.Events;

public class CharactersHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    private float _health;
    private bool _isDead;
    public float MaxHealth => _maxHealth;
    public float Health => _health;
    public bool IsDead => _isDead;

    public UnityEvent DamageMe;
    public UnityEvent AnimationDead;

    private void Awake()
    {
        _health = _maxHealth;
        _isDead = false;
    }

    public void TakeHealth(int pills)
    {
        _health += pills;

        if (_health >= _maxHealth)
            _health = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;
            _isDead = true;
            AnimationDead?.Invoke();
            Destroy(this.gameObject, 1f);
        }

        DamageMe?.Invoke();
    }
}
