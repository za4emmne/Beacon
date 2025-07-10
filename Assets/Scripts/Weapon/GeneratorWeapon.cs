using UnityEngine;

public class GeneratorWeapon : Spawner<Weapon>
{
    private Coroutine _secondCoroutine;
    private Transform _target;

    private void Start()
    {
        base.OnStartGenerator();
    }

    public void GetTarget(Transform target)
    {
        _target = target;
    }

    public void ChangedSpawnDelay(int level)
    {
        minDelay /= level;
        maxDelay /= level;
        maxDelay *= 1.6f;
    }

    public void OnStartSecondGeneration()
    {
        if (_secondCoroutine == null)
        {
            _secondCoroutine = StartCoroutine(Spawn());
        }
    }

    public override Weapon GetObject()
    {
        var weapon = base.GetObject();
        weapon.Initialize();
        weapon.InitGenerator(this);

        if (_target != null)
        {
            weapon.GetTarget(_target);
        }

        return weapon;
    }

    public override void PutObject(Weapon obj)
    {
        base.PutObject(obj);
        obj.SetZeroDirection();
    }

    protected override Vector3 PositionGeneraton()
    {
        return _transform.position;
    }
}
