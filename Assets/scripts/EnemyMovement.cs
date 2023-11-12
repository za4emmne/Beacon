using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 0.1f;
    //[SerializeField] private Target _target;
    [SerializeField] private SpawnSkeleton _spawnSkeletonLink;

    private const string _animationNameRun = "Run";
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _spawnSkeletonLink.GetTarget().position, _speed * Time.deltaTime);
        AnimationRun();
    }

    private void AnimationRun()
    {
        _animator.SetTrigger(_animationNameRun);
    }
}