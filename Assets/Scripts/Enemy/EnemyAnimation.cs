using System.Collections;
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

    [SerializeField] private GameObject _dust;

    private EnemyAttacked _enemyAttacked;
    private EnemyHealth _health;
    private EnemyMovement _enemyMovement;
    private Enemy _enemy;
    public GameObject _deathDustPrefab;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Color _color;
    private Coroutine _coroutine;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyAttacked = GetComponent<EnemyAttacked>();
        _health = GetComponent<EnemyHealth>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _animator = GetComponent<Animator>();
        Color _color = _spriteRenderer != null ? _spriteRenderer.color : Color.white;
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
        //_dust.SetActive(true);
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

    private IEnumerator IAnimateHit()
    {
        for (int i = 0; i < 6; i++)
        {
            //_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(.1f);
            _spriteRenderer.color = Color.white;
            //_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(.1f);
        }

    }
}
