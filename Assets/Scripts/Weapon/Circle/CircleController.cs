using System;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CircleController : WeaponController
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private int _addSpeed;

    protected override void Awake()
    {
        base.Awake();
        _prefabs[1].SetActive(false);
    }

    protected override void Level2(int level)
    {
        base.Level2(level);
        data.CurrentSpeed += _addSpeed;
        CircleBehavior circle = _prefabs[0].GetComponent<CircleBehavior>();
        circle.Initialize();
    }

    protected override void Level3(int level)
    {
        base.Level3(level);
        _prefabs[1].SetActive(true);
    }

    protected override void Level4(int level)
    {
        base.Level3(level);

    }
}
