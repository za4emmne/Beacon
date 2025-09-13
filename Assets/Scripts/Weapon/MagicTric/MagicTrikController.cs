using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class MagicTrikController : WeaponController
{
    private GeneratorWeapon _generator;

    protected override void Awake()
    {
        base.Awake();
        _generator = GetComponent<GeneratorWeapon>();
    }

    protected override void Level2(int level)
    {
        base.Level2(level);
        _generator.ChangedSpawnDelay(level);
    }

    protected override void Level3(int level)
    {
        base.Level3(level);
        _generator.OnStartSecondGeneration();
    }

    protected override void Level4(int level)
    {
        base.Level4(level);
        _generator.ChangedSpawnDelay(level);
    }
}
