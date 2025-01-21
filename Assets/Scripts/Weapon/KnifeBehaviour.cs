using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//поведение ноаж, как и куда летит

public class KnifeBehaviour : ProjectileWeaponBehaviour
{
    private KnifeController kc;
    private float _horizontalMove;
    private float _verticalMove;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();
        kc = FindObjectOfType<KnifeController>();
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime);
    }
}
