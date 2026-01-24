using System;
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] protected int level;
    [SerializeField] public WeaponData data;

    protected Coroutine fireCoroutine;
    protected GeneratorWeapon generator;

    protected string _description;

    public string Description => _description;

    protected virtual void Awake()
    {

    }

    protected virtual IEnumerator FireLoop()
    {
        while (enabled)
        {
            generator.SpawnProjectilesBurst();
            yield return new WaitForSeconds(data.CurrentDelay);
        }
    }

    public virtual void Initialize(WeaponData weaponData)
    {
        data = weaponData;
        level = data.CurrentLevel;
        Upgraid(level);
        weaponData.CurrentDescription = _description;
    }

    private void Upgraid(int level)
    {
        switch (level)
        {
            case 2:
                Level2(level); break;
            case 3:
                Level3(level); break;
            case 4:
                Level4(level); break;
        }
    }

    protected virtual void Level2(int level)
    {
        UpText();
    }

    protected virtual void Level3(int level)
    {
        UpText();
    }

    protected virtual void Level4(int level)
    {
        UpText();
    }

    private void UpText()
    {
        Debug.Log(data.name + " улучшено до " + level + " уровня");
    }
}
