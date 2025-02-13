using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SableController : Weapon
{
    private const string AnimationNameAttack = "Attack";

    private Animator _animator;
    private PolygonCollider2D _polygonCollider;
    private Coroutine _coroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
        _coroutine = StartCoroutine(AnimatorController());
        _polygonCollider.enabled = false;
    }

    private IEnumerator AnimatorController()
    {
        var wait = new WaitForSeconds(_delay);

        while (enabled)
        {
            _animator.SetTrigger(AnimationNameAttack);
            
            _polygonCollider.enabled = true;
            yield return wait;
            _polygonCollider.enabled = false;
        }
    }
}
