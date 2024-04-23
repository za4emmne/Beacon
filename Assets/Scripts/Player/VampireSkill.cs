using UnityEngine;
using System.Collections;

public class VampireSkill : MonoBehaviour
{
    [SerializeField] private float _vampireIndex;
    [SerializeField] private CharactersHealth _health;

    private int _delay = 1;
    private Coroutine _coroutine;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (Input.GetKey(KeyCode.Z))
            {
                _coroutine = StartCoroutine(Vampired(enemy));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
    }

    private IEnumerator Vampired(Enemy enemy)
    {
        var waitForAnySecond = new WaitForSeconds(_delay);

        for (int i = 0; i < 6; i++)
        {
            enemy.GetComponent<CharactersHealth>().TakeDamage(_vampireIndex);
            _health.TakePills(_vampireIndex);
            yield return new WaitForSeconds(_delay);
        }
    }
}
