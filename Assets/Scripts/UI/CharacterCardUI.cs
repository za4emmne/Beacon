using UnityEngine;
using UnityEngine.UI;

public class CharacterCardUI : MonoBehaviour
{
    public Image iconImage;
    public Text characterNameText;
    public Image startedWeaponImage;
    public Text priceText;
    public Button buyButton;
    public Text buyText;
    public Button selectButton;
    public Text selectText;
    public GameObject selectedMarker;

    [HideInInspector] public string key;
    [HideInInspector] public CharacterData characterData;
}
