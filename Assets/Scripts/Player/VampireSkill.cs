using UnityEngine;
using System.Collections;

public class VampireSkill : MonoBehaviour
{
    [SerializeField] private int _vampireIndex;
    [SerializeField] private CharactersHealth _health;

    private int _delay = 100;
    private Coroutine _coroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy) && enemy != null)
        {
            if(Input.GetKey(KeyCode.Z))
            _coroutine = StartCoroutine(Vampired(enemy));
        }
    }

    private IEnumerator Vampired(Enemy enemy)
    {
        var waitForAnySecond = new WaitForSeconds(_delay);

        for (int i = 0; i < 3; i++)
        {
            enemy.GetComponent<CharactersHealth>().TakeDamage(_vampireIndex);
            _health.TakePills(_vampireIndex);
            yield return new WaitForSeconds(1f);
        }
    }
}
