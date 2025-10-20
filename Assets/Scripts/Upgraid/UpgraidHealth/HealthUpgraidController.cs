using UnityEngine;

public class HealthUpgraidController : WeaponController
{
    private PlayerHealth _playerHealth;
    private int _count = 10;

    public override void Initialize(WeaponData weaponData)
    {
        base.Initialize(weaponData);
        _playerHealth = Player.singleton.GetComponent<PlayerHealth>();
        _playerHealth.UpgraidMaxHealth(_count);
    }
    protected override void Level2(int level)
    {
        base.Level2(level);

        _playerHealth.UpgraidMaxHealth(_count);
    }

    protected override void Level3(int level)
    {
        base.Level3(level);
        _playerHealth.UpgraidMaxHealth(_count);
    }
}
