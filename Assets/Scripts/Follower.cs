using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _targetCharacter;
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        transform.position = _targetCharacter.position + _offset;
    }
}
