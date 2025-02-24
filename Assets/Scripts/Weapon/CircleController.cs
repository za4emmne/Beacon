using System;
using System.Drawing;
using UnityEngine;

public class CircleController : Weapon
{
    private Vector3 _axis;
    private Vector3 _point;

    private void Start()
    {
        base.Initialize();
        _axis = new Vector3(0, 0, 1);
    }

    [Obsolete]
    private void Update()
    {
        _point = Player.singleton.transform.position;
        transform.RotateAround(_point, _axis, Time.deltaTime * speed);
    }
}
