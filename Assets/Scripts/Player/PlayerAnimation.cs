using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animator))]

public class PlayerAnimation : MonoBehaviour
{
    private const string AnimationNameDead = "Dead";
    private const string AnimationNameAttack = "Attack";
    private const string AnimationNameRun = "Run";

    private PlayerHealth _characters;
    private PlayerMovement _playerMovenment;
    private UIManager _uiManager;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _coroutine;
    private Color _color;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _color = _spriteRenderer.color;
    }

    private void OnDisable()
    {
        _characters.Died -= OnDeadAnimation;
        //_playerAttack.Attacked -= OnAttackAnimation;
        _playerMovenment.Run -= OnRunAnimation;
    }

    public void Initialize()
    {
        _characters = Player.singleton.GetComponent<PlayerHealth>();
        _playerMovenment = Player.singleton.GetComponent<PlayerMovement>();
        _characters.Died += OnDeadAnimation;
        _playerMovenment.Run += OnRunAnimation;
    }

    public void GameOver()
    {
        UIManager.Instance.OnDeadScreenActivate();
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
