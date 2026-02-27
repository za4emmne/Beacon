using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController
{
    protected override void Awake()
    {
        base.Awake();
        generator = GetComponent<GeneratorWeapon>();
        _description = LocalizationManager.Instance.GetTranslation("knifeFirstUpDescription");
        generator.SetProjectilesPerShot(1);
    }

    private void Start()
    {
        generator.InitSpawnDelay(data.CurrentDelay, data.CurrentDelay);
        fireCoroutine = StartCoroutine(FireLoop());
    }

    protected override void Level2(int level)  //появляется второй снаряд
    {
        base.Level2(level);
        _description = LocalizationManager.Instance.GetTranslation("knifeSecondUpDescription");
        generator.SetProjectilesPerShot(2);
    }

    protected override void Level3(int level)  //уменьшается скорость перезарядки
    {
        base.Level3(level);
        generator.ChangedSpawnDelay(level);
        generator.SetProjectilesPerShot(3);
        _description = LocalizationManager.Instance.GetTranslation("knifeThirdUpDescription");
    }

    protected override void Level4(int level)
    {
        base.Level4(level);
        generator.SetProjectilesPerShot(4);
    }
}
