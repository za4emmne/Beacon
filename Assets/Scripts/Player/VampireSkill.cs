using UnityEngine;

public class VampireSkill : MonoBehaviour
{
    [SerializeField] private int _vampireIndex = 1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("vampire");
                enemy.GetComponent<CharactersHealth>().TakeDamage(_vampireIndex);
            }

        }
    }
}
