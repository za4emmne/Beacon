using UnityEngine;

[RequireComponent(typeof(Animator))]

public class EnemyAnimation : MonoBehaviour
{
    private const string AnimationNameDead = "Dead";
    private const string AnimationNameAttack = "Attack";
    private const string AnimationNameRun = "Run";

    [SerializeField] private EnemyAttacked _enemyAttacked;
    [SerializeField] private CharactersHealth _characters;
    [SerializeField] private EnemyMovement _enemyMovement;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _enemyAttacked.AnimationAttackPlayed += AttackAnimation;
        _characters.AnimationDeadPlayed += DeadAnimation;
        _enemyMovement.AnimationRunPlayed += RunAnimation;
    }

    private void OnDisable()
    {
        _enemyAttacked.AnimationAttackPlayed -= AttackAnimation;
        _characters.AnimationDeadPlayed -= DeadAnimation;
        _enemyMovement.AnimationRunPlayed -= RunAnimation;
    }

    public void DeadAnimation()
    {
        _animator.SetTrigger(AnimationNameDead);
    }

    public void AttackAnimation()
    {
        _animator.SetTrigger(AnimationNameAttack);
    }

    public void RunAnimation()
    {
        _animator.SetTrigger(AnimationNameRun);
    }
}
