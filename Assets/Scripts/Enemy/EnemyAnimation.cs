using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(EnemyAttacked))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof (CharactersHealth))]

public class EnemyAnimation : MonoBehaviour
{
    private const string AnimationNameDead = "Dead";
    private const string AnimationNameAttack = "Attack";
    private const string AnimationNameRun = "Run";

    [SerializeField] private EnemyAttacked _enemyAttacked;
    [SerializeField] private CharactersHealth _characters;
    [SerializeField] private EnemyMovement _enemyMovement;

    private Animator _animator;

    private void Awake()
    {
        _enemyAttacked = GetComponent<EnemyAttacked>();
        _characters = GetComponent<CharactersHealth>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _enemyAttacked.Attacked += OnAttackAnimation;
        _characters.Died += OnDeadAnimation;
        _enemyMovement.AnimationRunPlayed += OnRunAnimation;
    }

    private void OnDisable()
    {
        _enemyAttacked.Attacked -= OnAttackAnimation;
        _characters.Died -= OnDeadAnimation;
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
}
