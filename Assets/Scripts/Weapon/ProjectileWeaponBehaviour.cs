using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//проверяет направление движения и полета

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    [SerializeField] private float _destroyAfterSeconds;

    protected Vector3 direction;

    protected virtual void Start()
    {
        Destroy(gameObject, _destroyAfterSeconds);
    }

    public void SetDirection(Vector3 directionPlayer)
    {
        direction = directionPlayer;
    }

}
