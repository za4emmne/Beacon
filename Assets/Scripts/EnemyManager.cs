using System.Collections;
using UnityEngine;
using System;

public class EnemyManager : PoolObject<EnemyViewer>
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _delay;
    [SerializeField] private float _delayBeforeRelease;
    [SerializeField] private StarGenerator _generator;

    public event Action oneKill;

    private void Start()
    {
        base.StartGeneration();
    }

    public override void OnGet(EnemyViewer spawnObject)
    {
        base.OnGet(spawnObject);
        spawnObject.Initialize(this);
    }

    public override void OnRelease(EnemyViewer spawnObject)
    {
        _generator.GetObjectAtPosition(spawnObject.transform.position);
        base.OnRelease(spawnObject);
        oneKill?.Invoke();
    }
}