using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class CharacterShop : MonoBehaviour
{
    [System.Serializable]
    public class CharacterItem
    {
        public string key;
        public Sprite icon;
        public int price;
        public Button buyButton;
        public Button selectButton;
        public Image characterImage;
        public GameObject selectedMarker;
        public Text priceText;
    }

    public List<CharacterItem> characters;
    public Text coinsText;

    void Start()
    {
        UpdateShopUI();
    }

    public void BuyCharacter(string characterKey)
    {
        var item = characters.Find(x => x.key == characterKey);
        if (item == null) return;

        if (!YG2.saves.unlockedCharacters.Contains(characterKey) && YG2.saves.coins >= item.price)
        {
            YG2.saves.coins -= item.price;
            YG2.saves.unlockedCharacters.Add(characterKey);
            YG2.saves.selectedCharacter = characterKey;
            YG2.SaveProgress();
            UpdateShopUI();
        }
    }

    public void SelectCharacter(string characterKey)
    {
        if (YG2.saves.unlockedCharacters.Contains(characterKey))
        {
            YG2.saves.selectedCharacter = characterKey;
            YG2.SaveProgress();
            UpdateShopUI();
        }
    }

    void UpdateShopUI()
    {
        coinsText.text = YG2.saves.coins.ToString();

        foreach (var item in characters)
        {
            // Иконка и цена
            item.characterImage.sprite = item.icon;
            item.priceText.text = item.price.ToString();

            bool unlocked = YG2.saves.unlockedCharacters.Contains(item.key);
            bool selected = item.key == YG2.saves.selectedCharacter;

            item.buyButton.interactable = !unlocked && YG2.saves.coins >= item.price;
            item.buyButton.gameObject.SetActive(!unlocked);
            item.selectButton.gameObject.SetActive(unlocked);
            item.selectedMarker.SetActive(selected);

            // Назначаем действия (убедитесь, что кнопки подписаны только 1 раз!)
            item.buyButton.onClick.RemoveAllListeners();
            item.buyButton.onClick.AddListener(() => BuyCharacter(item.key));
            item.selectButton.onClick.RemoveAllListeners();
            item.selectButton.onClick.AddListener(() => SelectCharacter(item.key));
        }
    }
}
