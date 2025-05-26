using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorWeapon : Spawner<Weapon>
{
    private Coroutine _secondCoroutine;

    private void Start()
    {
        base.OnStartGenerator();
    }

    public void ChangedSpawnDelay(int level)
    {
        minDelay /= level;
        maxDelay /= level;
        maxDelay *= 1.6f;
    }

    public void OnStartSecondGeneration()
    {
        if (_secondCoroutine == null)
        {
            _secondCoroutine = StartCoroutine(Spawn());
        }
    }

    public override Weapon GetObject()
    {
        var weapon = base.GetObject();
        weapon.Initialize();
        weapon.InitGenerator(this);

        return weapon;
    }

    public override void PutObject(Weapon obj)
    {
        base.PutObject(obj);
        obj.SetZeroDirection();
    }

    protected override Vector3 PositionGeneraton()
    {
        return transform.position;
    }
}
