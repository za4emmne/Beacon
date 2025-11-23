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
        _description = LocalizationManager.Instance.GetTranslation("sableFirstUpDescription");
        _firstSable = _prefabs[0].GetComponent<SableBehavior>();
        _secondSable = _prefabs[1].GetComponent<SableBehavior>();
        _prefabs[1].SetActive(false);
    }

    protected override void Level2(int level)
    {
        base.Level2(level);
        _description = LocalizationManager.Instance.GetTranslation("sableSecondUpDescription");
        _prefabs[1].SetActive(true);
        _firstSable.Initialize();
        _secondSable.Initialize();
    }

    protected override void Level3(int level)
    {
        base.Level3(level);
        _description = LocalizationManager.Instance.GetTranslation("sableThirdUpDescription");
        data.CurrentDamage += level;
        _firstSable.Initialize();
        _secondSable.Initialize();
    }

    protected override void Level4(int level)
    {
        base.Level3(level);
        data.CurrentDelay *= 0.9f;
        _firstSable.Initialize();
        _secondSable.Initialize();
    }
}
