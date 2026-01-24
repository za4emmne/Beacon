using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animator))]

public class PlayerAnimation : MonoBehaviour
{
    private const string AnimationNameDead = "Dead";
    private const string AnimationNameAttack = "Attack";
    private const string AnimationNameRun = "Run";
    private const string AnimationNameHurt = "Hurt";
    private const string AnimationNameRecover = "Recover";

    private PlayerHealth _characters;
    private PlayerMovement _playerMovenment;
    private PlayerWeapons _weapons;
    private UIManager _uiManager;
    private Rigidbody2D _rigidbody;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _coroutine;
    private Color _color;
    private float _delay;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _color = _spriteRenderer.color;
    }

    private void Start()
    {
        _coroutine = null;
        StartCoroutine(IAnimateAttack());
    }

    private void Update()
    {
        if (_playerMovenment != null)
        {
            float speed = _rigidbody.linearVelocity.magnitude;
            _animator.SetFloat(AnimationNameRun, speed);
        }
    }

    private void OnDisable()
    {
        _characters.Died -= OnDeadAnimation;
    }

    public void Initialize()
    {
        _characters = Player.singleton.GetComponent<PlayerHealth>();
        _playerMovenment = Player.singleton.GetComponent<PlayerMovement>();
        _weapons = Player.singleton.gameObject.GetComponent<PlayerWeapons>();
        _characters.Died += OnDeadAnimation;
        UpdateDelay();
    }

    public void UpdateDelay()
    {
        _delay = _weapons.StartWeapon.CurrentDelay;
    }

    public void GameOver()
    {
        UIManager.Instance.OnDeadScreenActivate();
        Time.timeScale = 0f;
    }

    public void OnRecoverAnimation()
    {
        _animator.SetTrigger(AnimationNameRecover);
    }


    public void OnDeadAnimation()
    {
        _animator.SetTrigger(AnimationNameDead);
    }

    public void OnAttackAnimation()
    {
        _animator.SetTrigger(AnimationNameAttack);
    }

    public void OnGetDamageAnimation()
    {
        //_animator.SetTrigger(AnimationNameHurt);
        if (_coroutine == null)
            _coroutine = StartCoroutine(IAnimateHit());
        else _coroutine = null;
    }

    private IEnumerator IAnimateAttack()
    {
        while (enabled)
        {
            OnAttackAnimation();
            yield return new WaitForSeconds(_delay);
        }
    }

    private IEnumerator IAnimateHit()
    {
        for (int i = 0; i < 4; i++)
        {
            //_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0f);
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.1f);
            _spriteRenderer.color = _color;
            //_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(.1f);
        }

        _coroutine = null;
    }
}
