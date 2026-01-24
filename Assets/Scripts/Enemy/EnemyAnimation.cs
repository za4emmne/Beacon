using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyAttacked))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyAnimation : MonoBehaviour
{
    private const string AnimationNameDead = "Dead";
    private const string AnimationNameAttack = "Attack";
    private const string AnimationNameRun = "Run";
    private const string AnimationNameHit = "Hit";

    [Header("Death VFX")]
    [SerializeField] private float _dissolveSpeed = 2f;
    [ColorUsage(true, true)][SerializeField] private Color _dissolveOutColor = Color.cyan;
    [ColorUsage(true, true)][SerializeField] private Color _dissolveInColor = Color.green;

    private Animator _animator;
    private EnemyAttacked _enemyAttacked;
    private EnemyHealth _health;
    private EnemyMovement _enemyMovement;
    private SpriteRenderer _spriteRenderer;
    private Material _mat;
    private Coroutine _deathCoroutine;
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyAttacked = GetComponent<EnemyAttacked>();
        _health = GetComponent<EnemyHealth>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _animator = GetComponent<Animator>();

        // экземпл€р материала дл€ этого врага
        _mat = _spriteRenderer.material;
        _mat.SetFloat("_DissolveAmount", 1f);           // 1 Ч полностью видим
        _mat.SetColor("_DissolveColor", _dissolveOutColor);
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

        if (_deathCoroutine == null)
            _deathCoroutine = StartCoroutine(DieWithDissolve());
    }

    public void OnAttackAnimation() => _animator.SetTrigger(AnimationNameAttack);
    public void OnRunAnimation() => _animator.SetTrigger(AnimationNameRun);
    public void OnHitAnimation() => _animator.SetTrigger(AnimationNameHit);

    private IEnumerator DieWithDissolve()
    {
        float dissolveAmount = 1f;       // начало: враг видим
        _mat.SetColor("_DissolveColor", _dissolveOutColor);

        // отключаем коллайдер/логику, чтобы мЄртвый враг не мешал
        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;
        _enemyMovement.enabled = false;

        while (dissolveAmount > -0.1f)
        {
            dissolveAmount -= Time.deltaTime * _dissolveSpeed;
            _mat.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }

        _enemy.OnRelease();
    }
}
