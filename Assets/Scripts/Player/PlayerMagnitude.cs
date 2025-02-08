using UnityEngine;

public class PlayerMagnitude : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Star>(out Star star))
        {
            star.Add();
        }
    }
}
