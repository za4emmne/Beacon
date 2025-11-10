using UnityEngine;

public class SpeedUpgraidController : WeaponController
{
    private PlayerMovement _playerMovement;
    private float _count = 10;

    public override void Initialize(WeaponData weaponData)
    {
        base.Initialize(weaponData);
        _playerMovement = Player.singleton.GetComponent<PlayerMovement>();
        _playerMovement.UpgradeSpeed(_count);
    }
    protected override void Level2(int level)
    {
        base.Level2(level);

        _playerMovement.UpgradeSpeed(_count);
    }

    protected override void Level3(int level)
    {
        base.Level3(level);
        _playerMovement.UpgradeSpeed(_count);
    }
}
