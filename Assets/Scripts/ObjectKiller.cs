using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectKiller : MonoBehaviour
{
    [SerializeField] private WeaponGenerator _generator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Weapon>(out Weapon weapon))
        {
            _generator.PutObject(weapon);
        }
    }
}
