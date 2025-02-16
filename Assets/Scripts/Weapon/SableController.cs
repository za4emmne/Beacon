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

    private void Start()
    {
        base.Initialize();
        _coroutine = StartCoroutine(AnimatorController());
        _polygonCollider.enabled = false;
    }

    private IEnumerator AnimatorController()
    {
        var wait = new WaitForSeconds(_delay);
        var delayWeapon = new WaitForSeconds(1f);

        while (enabled)
        {
            _animator.SetTrigger(AnimationNameAttack);
            
            _polygonCollider.enabled = true;
            yield return wait;
            _polygonCollider.enabled = false;
        }
    }
}
