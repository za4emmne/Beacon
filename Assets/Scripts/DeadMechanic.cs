using UnityEngine;

public class DeadMechanic : MonoBehaviour
{
    [SerializeField] private CharactersHealth _characters;

    private void OnEnable()
    {
        _characters.Died += OnDead;
    }

    private void OnDisable()
    {
        _characters.Died -= OnDead;
    }

    private void OnDead()
    {
        Destroy(this.gameObject, 0.7f);
    }
}
