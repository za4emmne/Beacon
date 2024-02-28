using UnityEngine;

[RequireComponent(typeof(Animator))]

public class PlayerAnimation : MonoBehaviour
{
    private const string AnimationNameDead = "Dead";
    private const string AnimationNameAttack = "Attack";
    private const string AnimationNameRun = "Run";

    [SerializeField] private PlayerMovenment _playerMovenment;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
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
        _animator.SetFloat(AnimationNameRun, Mathf.Abs(_playerMovenment.VerticalMove + _playerMovenment.HorizontalMove));
    }
}
