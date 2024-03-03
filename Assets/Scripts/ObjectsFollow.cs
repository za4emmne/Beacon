using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]

public class ObjectsFollow : MonoBehaviour
{
    [SerializeField] private Transform _targetCharacter;
    [SerializeField] private Vector3 _offset;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>(); 
    }

    private void Update()
    {
        //if (_targetCharacter != null)
        //    _rectTransform.anchoredPosition = _targetCharacter.position;
        //transform.rotation = Quaternion.LookRotation(transform.position - _targetCharacter.position);
        transform.position = _targetCharacter.position + _offset;
    }
}
