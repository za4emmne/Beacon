using UnityEngine;

public class CooldownController : WeaponController
{
    private PlayerWeapons _player;

    public override void Initialize(WeaponData weaponData)
    {
        base.Initialize(weaponData);
        _player = Player.singleton.GetComponent<PlayerWeapons>();
    }

    protected override void Level2(int level)
    {
        Debug.Log(_player.StartWeapon.CurrentDelay);
        base.Level2(level);
        _player.StartWeapon.CurrentDelay *= 1.1f;
        _player.StartWeapon.Controller.Initialize(_player.StartWeapon);
        Debug.Log(_player.StartWeapon.CurrentDelay);
    }

    protected override void Level3(int level)
    {
        base.Level3(level);

    }
}
