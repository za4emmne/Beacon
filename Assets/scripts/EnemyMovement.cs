using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private SpawnEnemy _spawnEnemyLink;

    private const string AnimationNameRun = "Run";

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            _spawnEnemyLink.GetTarget().position, _speed * Time.deltaTime);
        AnimationRun();
    }

    private void AnimationRun()
    {
        _animator.SetTrigger(AnimationNameRun);
    }
}