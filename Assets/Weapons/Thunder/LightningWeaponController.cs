using UnityEngine;

public class LightningWeaponController : WeaponController
{
    protected override void Awake()
    {
        base.Awake();
        _description = "Увеличивается урон и радиус поражения";
    }

    protected override void Level2(int level)
    {
        base.Level2(level);

        // Увеличиваем урон и радиус
        data.CurrentDamage *= 1.5f;
        data.CurrentAttackRange *= 1.2f;

        _description = "Увеличивается урон и количество цепей";
        //_description = $"Урон увеличен до {data.CurrentDamage}. Радиус поражения увеличен.";
    }

    protected override void Level3(int level)
    {
        base.Level3(level);

        // Добавляем ещё одну цепь
        data.CurrentSpeed += 1; // Speed используется как количество цепей
        data.CurrentDamage *= 1.5f;

        _description = "Увеличивается урон и количество цепей. МАКСИМАЛЬНЫЙ УРОН";
        //_description = $"Урон увеличен до {data.CurrentDamage}. Молния бьёт {data.CurrentSpeed} целей.";
    }

    protected override void Level4(int level)
    {
        base.Level4(level);

        // Максимальная мощь
        data.CurrentDamage *= 2f;
        data.CurrentSpeed += 1;
        data.CurrentDelay *= 0.8f; // Уменьшаем кулдаун

        //_description = $"Урон увеличен до {data.CurrentDamage}. Молния бьёт {data.CurrentSpeed} целей. Кулдаун уменьшен.";
    }
}
