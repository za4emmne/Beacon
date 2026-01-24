using UnityEngine;
using System.Collections;

public class SwordController : WeaponController
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

    protected override IEnumerator FireLoop()
    {
        while (enabled)
        {
            generator.SpawnSwordBurst();
            yield return new WaitForSeconds(data.CurrentDelay);
        }
    }

    protected override void Level2(int level)  //появляется второй снаряд
    {
        base.Level2(level);
        generator.SetSwordSpreadAngle(30f);
        generator.SetProjectilesPerShot(2);

    }

    protected override void Level3(int level)  //уменьшается скорость перезарядки
    {
        base.Level3(level);
        generator.ChangedSpawnDelay(level);
        generator.SetProjectilesPerShot(3);
        //_generator.OnStartSecondGeneration();
    }

    protected override void Level4(int level)
    {
        base.Level4(level);
        generator.SetProjectilesPerShot(4);
    }
}
