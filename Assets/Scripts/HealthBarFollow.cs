using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    [SerializeField] private Transform _targetCharacter;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>(); 
    }

    private void Update()
    {
        if (_targetCharacter != null)
            _rectTransform.anchoredPosition = _targetCharacter.localPosition;
    }
}
