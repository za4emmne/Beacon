using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 0.1f;

    public EnemySpawn enemySpawn;
    private Transform _target;
    private const string _animationNameRun = "Run";
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //_target = GameObject.FindObjectOfType <EnemySpawn>().Target;
        _target = enemySpawn.Target;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        AnimationRun();
    }

    private void AnimationRun()
    {
        _animator.SetTrigger(_animationNameRun);
    }
}
