using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalletController : WeaponController
{


    protected override void Awake()
    {
        base.Awake();
        generator = GetComponent<GeneratorWeapon>();
        generator.SetProjectilesPerShot(1);
    }

    private void Start()
    {
        generator.InitSpawnDelay(data.CurrentDelay, data.CurrentDelay);
        fireCoroutine = StartCoroutine(FireLoop());
    }

    protected override void Level2(int level)
    {
        base.Level2(level);
        generator.SetProjectilesPerShot(level);
    }

    protected override void Level3(int level)
    {
        base.Level3(level);
        generator.OnStartSecondGeneration();
    }

    protected override void Level4(int level)
    {
        base.Level4(level);
        generator.SetProjectilesPerShot(level);
    }
}
