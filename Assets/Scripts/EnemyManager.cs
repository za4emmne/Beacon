using System.Collections;
using UnityEngine;

public class EnemyManager : PoolObject<EnemyViewer>
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _delay;
    [SerializeField] private float _delayBeforeRelease;

    private void Start()
    {
        base.StartGeneration();
        //StartCoroutine(GetPlayerTransform());
    }

    //private IEnumerator GetPlayerTransform()
    //{
    //    WaitForSeconds waitUpdate = new WaitForSeconds(_delay);

    //    while (enabled)
    //    {
    //        Transform target = _target;

    //        if (ActiveObject != null)
    //        {
    //            foreach (var enemy in ActiveObject)
    //            {
    //                enemy.GetComponent<EnemyMovement>().TakeTargetPosition(target);
    //            }
    //        }

    //        yield return waitUpdate;
    //    }
    //}

    public void CheckHealth(CharactersHealth health)
    {
        if (health.Current <= 0)
        
        { 
            StartCoroutine(OnReleaseWait(health));         
        }
    }

    private IEnumerator OnReleaseWait(CharactersHealth health)
    {
        WaitForSeconds waitRelease = new WaitForSeconds(_delayBeforeRelease);
        health.GetComponent<EnemyAnimation>().OnDeadAnimation();
        yield return waitRelease;
        OnRelease(health.GetComponent<EnemyViewer>()); 
    }
}