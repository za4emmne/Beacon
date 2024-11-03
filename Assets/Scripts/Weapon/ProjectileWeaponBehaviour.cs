using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    [SerializeField] protected Vector3 _direction;

    void Start()
    {
        
    }

public void SetDirectrion(Vector3 direction)
    {
        direction = _direction;
    }
}
