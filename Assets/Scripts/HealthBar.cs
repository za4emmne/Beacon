using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Start()
    {
        
    }

    public void ChangeValue(int health)
    {
        _slider.value = health;
    }

    public void SetMaxValue(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
    }
}
