using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class HealthText : MonoBehaviour
{
    [SerializeField] private PlayerHealth _player;

    private Text _healthText;

    private void Start()
    {
        _healthText = GetComponent<Text>();
        _healthText.text = _player.Health.ToString() + "/" + _player.MaxHealth.ToString();
    }

    public void ChangeHealth()
    {
        _healthText.text = _player.Health.ToString() + "/" + _player.MaxHealth.ToString();
    }
}
