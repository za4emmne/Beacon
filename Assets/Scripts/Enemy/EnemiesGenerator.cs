using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : Spawner<Enemy>
{
    [SerializeField] private StarGenerator _starGenerator;

    public event Action oneKill;

    private void Start()
    {
        //  base.OnStartGenerator();
    }

    public override Enemy GetObject()
    {
        var enemy = base.GetObject();
        enemy.Initialize(this);

        return enemy;
    }

    public override void PutObject(Enemy obj)
    {
        oneKill?.Invoke();
        var star = _starGenerator.GetObject();
        star.transform.position = obj.transform.position;
        base.PutObject(obj);
    }

}
