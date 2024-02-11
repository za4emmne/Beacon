using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerHealth))]

public class SmoothHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private float _stepHealth = 10f;

    public void SetMaxValue(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
    }

    public IEnumerator ChangeValue(int health)
    {
        while (_slider.value != health)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, health, _stepHealth * Time.deltaTime); 
            yield return null;
        }       
    }
}
