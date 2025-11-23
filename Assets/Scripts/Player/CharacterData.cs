using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class CharacterData : ScriptableObject
{
    public string characterKey;
    public Sprite icon;
    public int price;
    public bool isDefault;
}