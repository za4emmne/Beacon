using UnityEngine;

public class DeadMechanic : MonoBehaviour
{
    [SerializeField] private CharactersHealth _characters;

    private void OnEnable()
    {
        _characters.Died += OnDied;
    }

    private void OnDisable()
    {
        _characters.Died -= OnDied;
    }

    private void OnDied()
    {
        Destroy(gameObject, 0.7f);
    }
}
