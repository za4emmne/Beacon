using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : Spawner<Weapon>
{
    private void Start()
    {
        base.OnStartGenerator();
    }

    public override Weapon GetObject()
    {
        var weapon = base.GetObject();
        weapon.Initialize();

        return weapon;

    }

    protected override Vector3 PositionGeneraton()
    {
        return transform.position;
    }
}
