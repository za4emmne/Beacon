using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SableController : WeaponController
{
    [SerializeField] private GameObject[] _prefabs;

    private SableBehavior _firstSable;
    private SableBehavior _secondSable;

    protected override void Awake()
    {
        base.Awake();
        _firstSable = _prefabs[0].GetComponent<SableBehavior>();
        _secondSable = _prefabs[1].GetComponent<SableBehavior>();
        _prefabs[1].SetActive(false);
    }

    protected override void Level2(int level)
    {
        base.Level2(level);
        _prefabs[1].SetActive(true);
        _firstSable.Initialize();
        _secondSable.Initialize();
    }

    protected override void Level3(int level)
    {
        base.Level3(level);
        data.Damage += level;
        _firstSable.Initialize();
        _secondSable.Initialize();
    }

   
}
