using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyAttacked))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyHealth))]

public class EnemyAnimation : MonoBehaviour
{
    private const string AnimationNameDead = "Dead";
    private const string AnimationNameAttack = "Attack";
    private const string AnimationNameRun = "Run";
    private const string AnimationNameHit = "Hit";

    private EnemyAttacked _enemyAttacked;
    private EnemyHealth _health;
    private EnemyMovement _enemyMovement;

    private Animator _animator;

    private void Awake()
    {
        _enemyAttacked = GetComponent<EnemyAttacked>();
        _health = GetComponent<EnemyHealth>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _enemyAttacked.Attacked += OnAttackAnimation;
        _health.Died += OnDeadAnimation;
        _health.Changed += OnHitAnimation;
        _enemyMovement.AnimationRunPlayed += OnRunAnimation;
    }

    private void OnDisable()
    {
        _enemyAttacked.Attacked -= OnAttackAnimation;
        _health.Died -= OnDeadAnimation;
        _health.Changed -= OnHitAnimation;
        _enemyMovement.AnimationRunPlayed -= OnRunAnimation;
    }

    public void OnDeadAnimation()
    {
        _animator.SetTrigger(AnimationNameDead);
    }

    public void OnAttackAnimation()
    {
        _animator.SetTrigger(AnimationNameAttack);
    }

    public void OnRunAnimation()
    {
        _animator.SetTrigger(AnimationNameRun);
    }

    public void OnHitAnimation()
    {
        _animator.SetTrigger(AnimationNameHit);
    }
}
