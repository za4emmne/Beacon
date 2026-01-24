using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class EnemyHealth : CharactersHealth
{
    private EnemyMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
    }

    public override void TakeDamage(float damage, Vector2 hitSourcePosition)
    {
        base.TakeDamage(damage, hitSourcePosition);

        GeneratorDamageText.Instance.ShowDamageText(hitSourcePosition, damage);
        _movement.KnockbackFromPlayer(Player.singleton.transform.position);
    }
}
