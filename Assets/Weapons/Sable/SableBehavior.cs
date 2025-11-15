using System.Collections;
using UnityEngine;

public class SableBehavior : Weapon
{
    private const string AnimationNameAttack = "Attack";

    private Animator _animator;
    private PolygonCollider2D _polygonCollider;
    private Coroutine _coroutine;
    private AudioSource _audioSource;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        base.Initialize();
        _coroutine = StartCoroutine(AnimatorController());
        _polygonCollider.enabled = false;
    }

    private IEnumerator AnimatorController()
    {
        var wait = new WaitForSeconds(delay);

        while (enabled)
        {
            _animator.SetTrigger(AnimationNameAttack);
            
            _polygonCollider.enabled = true;
            _audioSource.Play();

            yield return wait;
            _polygonCollider.enabled = false;
        }
    }
}
