using System;
using UnityEngine;

public class CircleController : Weapon
{
    private void Start()
    {
        base.Initialize();
    }

    [Obsolete]
    private void Update()
    {
        //transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (speed * Time.deltaTime));
    }
}
