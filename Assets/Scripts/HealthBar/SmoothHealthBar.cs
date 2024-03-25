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
        _characters.HealthChanged += OnChange;
    }

    private void OnDisable()
    {
        _characters.HealthChanged -= OnChange;
    }

    public void OnChange()
    {
        StartCoroutine(ChangeValue());
    }

    private IEnumerator ChangeValue()
    {
        while (_slider.value != _characters.Health/ _characters.MaxHealth)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _characters.Health / _characters.MaxHealth, _stepHealth * Time.deltaTime);
            yield return null;
        }       
    }
}
