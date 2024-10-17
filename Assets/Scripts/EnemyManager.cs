using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyManager : PoolObject<EnemyViewer>
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _delay;

    private void Start()
    {
        base.StartGeneration();
        StartCoroutine(GetPlayerTransform());
    }

    private IEnumerator GetPlayerTransform()
    {
        WaitForSeconds waitUpdate = new WaitForSeconds(_delay);

        while(enabled)
        {
            Transform target = _target;
            Debug.Log(target.position);

            if (ActiveObject != null)
            {
                foreach (var enemy in ActiveObject)
                {
                    enemy.GetComponent<EnemyMovement>().TakeTargetPosition(target);
                }
            }

            yield return waitUpdate;
        }
    }
}