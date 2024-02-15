using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerHealth))]

public class SmoothHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private float _stepHealth = 0.1f;

    public IEnumerator ChangeValue(float health)
    {
        while (_slider.value != health)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, health, _stepHealth * Time.deltaTime); 
            yield return null;
        }       
    }
}
