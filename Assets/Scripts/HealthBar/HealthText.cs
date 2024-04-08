using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class HealthText : MonoBehaviour
{
    [SerializeField] private CharactersHealth _player;

    private Text _healthText;

    private void Start()
    {
        _healthText = GetComponent<Text>();
        _healthText.text = _player.Current.ToString() + "/" + _player.MaxCurrent.ToString();
    }

    public void ChangeHealth()
    {
        _healthText.text = _player.Current.ToString() + "/" + _player.MaxCurrent.ToString();
    }
}
