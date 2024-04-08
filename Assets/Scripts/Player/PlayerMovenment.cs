using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMovenment : MonoBehaviour
{
    private const string NameDirectionHorizontal = "Horizontal";
    private const string NameDirectionVertical = "Vertical";

    [SerializeField] private float _speed = 3;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private float _horizontalMove;
    private float _verticalMove;

    public event Action Run;

    public float HorizontalMove => _horizontalMove;
    public float VerticalMove => _verticalMove;


    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw(NameDirectionHorizontal) * _speed;
        _verticalMove = Input.GetAxisRaw(NameDirectionVertical) * _speed;

        _rigidbody2D.velocity = new Vector2(_horizontalMove, _verticalMove);

        if (_horizontalMove < 0)
        {
            _spriteRenderer.flipX = true;
        }
        if (_horizontalMove > 0)
        {
            _spriteRenderer.flipX = false;
        }

        Run?.Invoke();
    }
}
