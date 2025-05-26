using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBehavior : Weapon
{
    private Transform _player;
    private float _radius = 2f;
    private float _currentAngle = 0f;
    private float _targetAngle = 0f;

    private void Update()
    {
        _player = Player.singleton.transform;
        _targetAngle += speed * Time.deltaTime;
        _currentAngle = Mathf.LerpAngle(_currentAngle, _targetAngle, 0.1f);

        float x = _player.position.x + Mathf.Cos(_currentAngle) * _radius;
        float y = _player.position.y + Mathf.Sin(_currentAngle) * _radius;

        transform.position = new Vector2(x, y);
        transform.right = (transform.position - _player.position).normalized;
    }
}
