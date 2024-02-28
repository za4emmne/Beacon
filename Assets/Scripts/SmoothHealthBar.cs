using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private CharactersHealth _player;

    private float _stepHealth = 0.1f;

    public void Change()
    {
        StartCoroutine(ChangeValue());
    }

    private IEnumerator ChangeValue()
    {
        while (_slider.value != _player.Health/ _player.MaxHealth)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _player.Health/_player.MaxHealth, _stepHealth * Time.deltaTime); 
            yield return null;
        }       
    }
}
