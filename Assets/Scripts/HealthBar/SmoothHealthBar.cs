using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private PlayerHealth _characters;
    [SerializeField] private float _stepHealth = 1f;

    private Coroutine _coroutine;

    public void Initialize(PlayerHealth player)
    {
        _characters = player;
    }

    private void OnEnable()
    {
        _characters.Changed += OnChange;
        _characters.RaiseMe += OnRaise;
    }

    private void OnDisable()
    {
        _characters.Changed += Stop;
        _characters.RaiseMe -= OnRaise;
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

    public void OnRaise()
    {
        _slider.value = _characters.Current / _characters.MaxCurrent;
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
