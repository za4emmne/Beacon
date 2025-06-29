using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] protected int level;
    [SerializeField] public WeaponData data;

    protected virtual void Awake()
    {
        level = 0;
    }

    public virtual void Initialize(WeaponData weaponData)
    {
        data = weaponData;
        level = data.CurrentLevel;
        Upgraid(level);
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
        Debug.Log(data.name + " - " + level);
    }

    protected virtual void Level3(int level)
    {
        Debug.Log(data.name + " - " + level);
    }

    protected virtual void Level4(int level)
    {
        Debug.Log(data.name + " - " + level);
    }
}
