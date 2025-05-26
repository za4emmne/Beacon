using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalletBehavior : Weapon
{
    private Rigidbody2D _rigidbody2D;
    private Vector2 _position;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void Initialize()
    {
        base.Initialize();
        AddForce();
    }

    private void Update()
    {
        transform.Rotate(0, 0, 1);
    }

    private void AddForce()
    {
        _position = new Vector2(RandomPositionX(), 1f);
        _rigidbody2D.AddForce(_position * speed, ForceMode2D.Impulse);
    }

    private float RandomPositionX()
    {
        float positionX = Random.Range(-0.5f, 0.5f);

        return positionX;
    }
}
