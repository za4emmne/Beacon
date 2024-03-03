using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharactersHealth))]

public class EnemyAttacked : MonoBehaviour
{
    [SerializeField] private CharactersHealth _charactersHealth;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _damage = 1.5f;
    [SerializeField] private float _startTimeAttack = 0.8f;

    public UnityEvent AnimationAttack;

    private float _attackTime = 0;

    private void Start()
    {
        _charactersHealth = GetComponent<CharactersHealth>();
    }

    private void Update()
    {
        if (_charactersHealth.IsDead == false)
        {
            Attacked();
        }
    }

    private void Attacked()
    {
        if (_attackTime <= 0)
        {
            Collider2D[] hitHeroes = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerLayer);

            foreach (var player in hitHeroes)
            {
                AnimationAttack?.Invoke();
                player?.GetComponent<CharactersHealth>().TakeDamage(_damage);
            }
            _attackTime = _startTimeAttack;
        }
        else
        {
            _attackTime -= Time.deltaTime;
        }
    }
}
