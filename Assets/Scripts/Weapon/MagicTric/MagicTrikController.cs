using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class MagicTrikController : WeaponController
{
    private GeneratorWeapon _generator;
    private float _detectedRadius;
    private LayerMask _targetLayer = 1 << 8;
    private float _waitToDetected = 1;

    protected override void Awake()
    {
        base.Awake();
        _generator = GetComponent<GeneratorWeapon>();
    }

    private void Start()
    {
        _detectedRadius = data.CurrentAttackRange;
        StartCoroutine(Detecting());
    }

    private IEnumerator Detecting()
    {
        var waitForSeconds = new WaitForSeconds(_waitToDetected);

        while (enabled)
        {
            FindTarget();
            yield return waitForSeconds;
        }
    }

    private Transform FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _detectedRadius, _targetLayer);

        if (colliders != null && colliders.Length > 0)
        {
            return colliders[0].transform;
        }
        else
        {
            return null;
        }
    }
}
