using UnityEngine;
using System;

[RequireComponent(typeof(CharactersHealth))]

public class EnemyAttacked : MonoBehaviour
{
    [SerializeField] private float _damage = 1.5f;
    //[SerializeField] private GameObject _effect;

    public event Action Attacked;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            //Instantiate(_effect, player.transform.position, Quaternion.identity);
            playerHealth.TakeDamage(_damage);
            player.GetComponent<PlayerAnimation>().OnGetDamageAnimation();
            Attacked?.Invoke();
        }
    }
}