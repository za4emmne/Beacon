using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private CharactersHealth _player;

    public void ChangeValue()
    {
        _slider.value = _player.Current/_player.MaxCurrent;
    }
}
