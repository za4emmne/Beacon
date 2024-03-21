using UnityEngine;

public class ObjectsFollow : MonoBehaviour
{
    [SerializeField] private Transform _targetCharacter;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        transform.position = _targetCharacter.position + _offset;
    }
}
