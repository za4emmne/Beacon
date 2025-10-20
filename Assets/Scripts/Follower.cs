using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _targetCharacter;
    [SerializeField] private Vector3 _offset;

    private void Start()
    {
        if (_targetCharacter == null)
        {
            Debug.LogError("_targetCharacter не назначен для Follower!", gameObject);
            return;
        }
    }

    public void Playertransform(Transform transform)
    {
        _targetCharacter = transform;
    }


    private void LateUpdate()
    {
        transform.position = _targetCharacter.position + _offset;
    }
}
