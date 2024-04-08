using UnityEngine;

public class PlayerTakePills : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Pills>(out Pills pills))
        {
            GetComponent<CharactersHealth>().TakePills(pills.Count);
            Destroy(pills);
        }
    }
}
