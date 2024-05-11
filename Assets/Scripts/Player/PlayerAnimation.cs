using UnityEngine;
using System.Collections;

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
    private SpriteRenderer _spriteRenderer;
    private Coroutine _coroutine;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        _animator.SetFloat(AnimationNameRun, Mathf.Abs(_playerMovenment.VerticalMove) + Mathf.Abs(_playerMovenment.HorizontalMove));
    }

    public void OnGetDamageAnimation()
    {
        _coroutine = StartCoroutine(IAnimateHit());
    }

    public void Stop()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator IAnimateHit()
    {
        for (int i = 0; i < 6; i++)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0f);
            yield return new WaitForSeconds(.1f);
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(.1f);
        }
    }
}
