using Unity.VisualScripting;
using UnityEngine;

public class YaropolkSableBehavior : Weapon
{
    private PolygonCollider2D _polygonCollider;

    private void Awake()
    {
        _polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        _polygonCollider.enabled = false;
    }

    public void OnColliderSable()
    {
        if (_polygonCollider.enabled == false)
            _polygonCollider.enabled = true;
    }

    public void OffColliderSable()
    {
        if (_polygonCollider.enabled == true)
            _polygonCollider.enabled = false;
    }
}
