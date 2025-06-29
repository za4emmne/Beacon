using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBehavior : Weapon
{
    [SerializeField] private float radius = 2f;

    private Transform _player;
    private Transform _independentPivot;
    private float currentAngle = 0f;

    private void Start()
    {
        _player = Player.singleton.transform;
        CreateIndependentPivot();
        PositionWeapon();
        base.Initialize();
    }

    private void CreateIndependentPivot()
    {
        // Создаем независимую точку поворота БЕЗ родительской связи с игроком
        GameObject pivot = new GameObject("IndependentWeaponPivot");
        _independentPivot = pivot.transform;

        // НЕ делаем pivot дочерним объектом игрока!
        _independentPivot.position = _player.position;

        // Делаем оружие дочерним объектом независимой точки поворота
        transform.SetParent(_independentPivot);
    }

    private void PositionWeapon()
    {
        transform.localPosition = new Vector3(radius, 0, 0);
    }

    private void Update()
    {
        // Обновляем позицию точки поворота, следуя за игроком
        _independentPivot.position = _player.position;

        // Вращаем точку поворота с постоянной скоростью
        _independentPivot.Rotate(0, 0, speed * Time.deltaTime);

        // Направляем оружие от центра
        Vector3 directionFromCenter = (transform.position - _player.position).normalized;
        transform.right = directionFromCenter;
    }
}
