using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerHealth))]

public class HealthText : MonoBehaviour
{
    [SerializeField] private Text _healthText;

    private PlayerHealth _player;

    private void Start()
    {
        _player = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        _healthText.text = _player.GetHealth.ToString() + "/" + _player.GetMaxHealth.ToString();
    }
}
