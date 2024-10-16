using UnityEngine;

[RequireComponent (typeof(CharactersHealth))]

public class DeadMechanic : MonoBehaviour
{
    private CharactersHealth _characters;

    private void Awake()
    {
        _characters = GetComponent<CharactersHealth>();
    }

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
        //сделать возвращение в пул
    }
}
