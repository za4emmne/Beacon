using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string AnimationNameRun = "Run";
    private const string AnimationNameIdle = "Idle";

    [SerializeField] private float _speed;
    [SerializeField] private int _health;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private HealthBarMathf _healthBarMathf;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private bool _isFaceRight = true;
    private float _horizontalMove;
    private int _maxHealth = 100;

    private void Start()
    {
        _health = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
        _healthBarMathf.SetMaxHealth(_maxHealth);
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _speed;
        _rigidbody2D.velocity = new Vector2(_horizontalMove, Input.GetAxisRaw("Vertical") * _speed);

        if (_horizontalMove < 0 && _isFaceRight)
        {
            Flip();
        }
        else if (_horizontalMove > 0 && !_isFaceRight)
        {
            Flip();
        }

        if (_horizontalMove != 0)
        {
            AnimationRun();
        }


        AnimationIdle();
        _healthBar.SetHealth(_health);
        _healthBarMathf.SetHealth(_health);
    }

    public void TakeHealth(int pills)
    {
        _health += pills;

        if (_health >= _maxHealth)
            _health = _maxHealth;
    }

    public void GetDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
            _health = 0;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public int GetHealth()
    {
        return _health;
    }

    private void Flip()
    {
        _isFaceRight = !_isFaceRight;

        Vector3 Scale = transform.localScale;
        Scale.x *= -1; 
        transform.localScale = Scale;
    }

    private void AnimationRun()
    {
        _animator.SetTrigger(AnimationNameRun);
    }
    private void AnimationIdle()
    {
        _animator.SetTrigger(AnimationNameIdle);
    }
}
