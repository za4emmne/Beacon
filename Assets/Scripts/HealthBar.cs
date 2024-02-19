using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerHealth _player;

    public void ChangeValue()
    {
        _slider.value = _player.Health/_player.MaxHealth;
    }
}
