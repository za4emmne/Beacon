using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttacked : MonoBehaviour
{
    public UnityEvent AnimationAttack;

    private int _damage = 5;
    private bool _isAttack;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharactersHealth player))
        {
            AnimationAttack?.Invoke();
            player.TakeDamage(_damage);
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent(out PlayerHealth player))
    //    {
    //        _isAttack = true;
    //        StartCoroutine(TakeDamage());
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.TryGetComponent(out PlayerHealth player))
    //    {
    //        _isAttack = false;
            
    //    }
    //}

    private IEnumerator TakeDamage()
    {
        var waitForAnySecond = new WaitForSeconds(2);
            yield return waitForAnySecond;
    }
}
