using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class CharacterData : ScriptableObject
{
    public string characterKey;
    public string characterName;
    public Sprite icon;
    public int price;
    public bool isDefault;
    public WeaponData startedWeapon;
    public GameObject playerPrefab;
}