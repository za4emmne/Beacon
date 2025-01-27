using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SableController : MonoBehaviour
{
    private const string AnimationNameAttack = "Attack";

    [SerializeField] private float _delay;

    private Animator _animator;
    private Coroutine _coroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _coroutine = StartCoroutine(AnimatorController());
    }

    private IEnumerator AnimatorController()
    {
        var wait = new WaitForSeconds(_delay);

        while (enabled)
        {
            _animator.SetTrigger(AnimationNameAttack);
            yield return wait;
        }
    }
}
