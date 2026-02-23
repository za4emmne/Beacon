using UnityEngine;
using System.Collections;

public class SwordController : WeaponController
{
    protected override void Awake()
    {
        base.Awake();
        generator = GetComponent<GeneratorWeapon>();
        generator.SetProjectilesPerShot(1);
        generator.SetSwordSpreadAngle(0f);
        generator.SetSwordOffsetDistance(0.4f);
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

    protected override void Level2(int level)
    {
        base.Level2(level);
        Debug.Log($"[SwordController] Level2 called, setting 2 swords");
        generator.SetProjectilesPerShot(2);
        generator.SetSwordSpreadAngle(20f);
        generator.SetSwordOffsetDistance(0.3f);
    }

    protected override void Level3(int level)
    {
        base.Level3(level);
        data.CurrentDelay *= 0.75f;
        generator.InitSpawnDelay(data.CurrentDelay, data.CurrentDelay);
    }

    protected override void Level4(int level)
    {
        base.Level4(level);
        generator.SetProjectilesPerShot(3);
        generator.SetSwordSpreadAngle(12f);
        generator.SetSwordOffsetDistance(0.25f);
    }
}
