using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] public Transform target;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        target = GameObject.FindObjectOfType<Player>().transform; //цель
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, _speed * Time.deltaTime); //преследование цели
        AnimationRun();
    }

    private void AnimationRun()
    {
        _animator.SetTrigger("Run");
    }
}
