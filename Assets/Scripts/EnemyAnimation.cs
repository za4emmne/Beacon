using UnityEngine;

[RequireComponent(typeof(Animator))]

public class EnemyAnimation : MonoBehaviour
{
    private const string AnimationNameDead = "Dead";
    private const string AnimationNameAttack = "Attack";

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void DeadAnimation()
    {
        _animator.SetTrigger(AnimationNameDead);
    }

    public void AttackAnimation()
    {
        _animator.SetTrigger(AnimationNameAttack);
    }
}
