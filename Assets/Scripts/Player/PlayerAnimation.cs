using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animator))]

public class PlayerAnimation : MonoBehaviour
{
    private const string AnimationNameDead = "Dead";
    private const string AnimationNameAttack = "Attack";
    private const string AnimationNameRun = "Run";

    [SerializeField] private PlayerHealth _characters;
    [SerializeField] private PlayerMovenment _playerMovenment;
    //[SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private UIManager _uiManager;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _coroutine;
    private Color _color;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _color = _spriteRenderer.color;
    }

    private void OnEnable()
    {
        _characters.Died += OnDeadAnimation;
        //_playerAttack.Attacked += OnAttackAnimation;
        _playerMovenment.Run += OnRunAnimation;
    }

    private void OnDisable()
    {
        _characters.Died -= OnDeadAnimation;
        //_playerAttack.Attacked -= OnAttackAnimation;
        _playerMovenment.Run -= OnRunAnimation;
    }

    public void GameOver()
    {
        _uiManager.OnDeadScreen();
        Time.timeScale = 0f;
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
        _animator.SetFloat(AnimationNameRun, Mathf.Abs(_playerMovenment.VerticalMove) + Mathf.Abs(_playerMovenment.HorizontalMove) + Mathf.Abs(_playerMovenment.JoystickCurrentVertical) + Mathf.Abs(_playerMovenment.JoystickCurrentHorizontal));
    }

    public void OnGetDamageAnimation()
    {
        _coroutine = StartCoroutine(IAnimateHit());
    }

    private IEnumerator IAnimateHit()
    {
        for (int i = 0; i < 6; i++)
        {
            //_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0f);
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.1f);
            _spriteRenderer.color = _color;
            //_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(.1f);
        }
    }
}
