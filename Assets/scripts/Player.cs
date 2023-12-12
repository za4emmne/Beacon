using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string AnimationNameRun = "Run";
    private const string AnimationNameIdle = "Idle";

    [SerializeField] private float _speed;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private bool _isFaceRight = true;
    private float _horizontalMove;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _speed;
        _rigidbody2D.velocity = new Vector2(_horizontalMove, Input.GetAxisRaw("Vertical") * _speed);

        if (_horizontalMove < 0 && _isFaceRight)
        {
            Flip();
        }
        else if (_horizontalMove > 0 && !_isFaceRight)
        {
            Flip();
        }

        if (_horizontalMove != 0)
        {
            AnimationRun();
        }


        AnimationIdle();
    }

    private void Flip()
    {
        _isFaceRight = !_isFaceRight;

        Vector3 Scale = transform.localScale;
        Scale.x *= -1; //поворачиваем по горизонтали
        transform.localScale = Scale; //присваиваем текущему положению уже повернутое
    }

    private void AnimationRun()
    {
        _animator.SetTrigger(AnimationNameRun);
    }
    private void AnimationIdle()
    {
        _animator.SetTrigger(AnimationNameIdle);
    }
}
