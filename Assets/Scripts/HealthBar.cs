using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void ChangeValue(float health)
    {
        _slider.value = health;
    }
}
