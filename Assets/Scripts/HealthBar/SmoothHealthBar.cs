using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private CharactersHealth _characters;
    [SerializeField] private float _stepHealth = 0.1f;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _characters.Changed += OnChange;
    }

    private void OnDisable()
    {
        _characters.Changed += Stop;
    }

    public void Stop()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public void OnChange()
    {
        _coroutine = StartCoroutine(ChangeValue());
    }

    private IEnumerator ChangeValue()
    {
        while (_slider.value != _characters.Current/ _characters.MaxCurrent)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _characters.Current / _characters.MaxCurrent, _stepHealth * Time.deltaTime);
            yield return null;
        }       
    }
}
