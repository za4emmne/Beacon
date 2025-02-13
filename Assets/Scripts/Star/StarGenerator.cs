using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : Spawner<Star>
{
    [SerializeField] private EnemiesGenerator _enemyManager;

    private void Start()
    {
        base.OnStartGenerator();
    }

    public override Star GetObject()
    {
        var star = base.GetObject();
        star.Initialize(this);


        star.gameObject.SetActive(true);

        return star;
    }
}
