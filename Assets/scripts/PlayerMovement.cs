using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Animator _animator;
    private const string _animationNameRun = "Run";
    private const string _animationNameIdle = "Idle";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            TurnRight();
            transform.Translate(_speed * Time.deltaTime, 0, 0);
            AnimationRun();
        }

        if (Input.GetKey(KeyCode.A))
        {
            TurnLeft();
            transform.Translate(_speed * Time.deltaTime * -1, 0, 0);
            AnimationRun();
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, _speed * Time.deltaTime, 0);
            AnimationRun();
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, _speed * Time.deltaTime * -1, 0);
            AnimationRun();
        }

        AnimationIdle();
    }

    private void TurnLeft()
    {
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
        }
    }

    private void TurnRight()
    {
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
        }
    }

    private void AnimationRun()
    {
        _animator.SetTrigger(_animationNameRun);
    }
    private void AnimationIdle()
    {
        _animator.SetTrigger(_animationNameIdle);
    }
}