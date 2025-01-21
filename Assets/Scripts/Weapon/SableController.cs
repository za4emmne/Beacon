using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SableController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedSable = Instantiate(_prefab);
        spawnedSable.transform.position = transform.position;
    }
}
