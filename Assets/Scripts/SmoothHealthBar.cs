using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private CharactersHealth _characters;
    [SerializeField] private float _stepHealth = 0.1f;

    private void OnEnable()
    {
        _characters.HealthChanged += Change;
    }

    private void OnDisable()
    {
        _characters.HealthChanged -= Change;
    }

    public void Change()
    {
        StartCoroutine(ChangeValue());
    }

    private IEnumerator ChangeValue()
    {
        while (_slider.value != _characters.Health/ _characters.MaxHealth)
        {
            //_slider.value = Mathf.MoveTowards(_slider.value, _characters.Health / _characters.MaxHealth, _stepHealth * Time.deltaTime);
            _slider.value = Mathf.Clamp(_characters.Health / _characters.MaxHealth, 0, _characters.MaxHealth);
            yield return null;
        }       
    }
}
