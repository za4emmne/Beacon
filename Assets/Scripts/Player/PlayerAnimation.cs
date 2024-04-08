using UnityEngine;

[RequireComponent(typeof(Animator))]

public class PlayerAnimation : MonoBehaviour
{
    private const string AnimationNameDead = "Dead";
    private const string AnimationNameAttack = "Attack";
    private const string AnimationNameRun = "Run";

    [SerializeField] private CharactersHealth _characters;
    [SerializeField] private PlayerMovenment _playerMovenment;
    [SerializeField] private PlayerAttack _playerAttack;

   private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _characters.Died += OnDeadAnimation;
        _playerAttack.Attacked += OnAttackAnimation;
        _playerMovenment.Run += OnRunAnimation;
    }

    private void OnDisable()
    {
        _characters.Died -= OnDeadAnimation;
        _playerAttack.Attacked -= OnAttackAnimation;
        _playerMovenment.Run -= OnRunAnimation;
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
        _animator.SetFloat(AnimationNameRun, Mathf.Abs(_playerMovenment.VerticalMove + _playerMovenment.HorizontalMove));
    }
}
