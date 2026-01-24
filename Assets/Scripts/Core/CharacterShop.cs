using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
using static UnityEditor.Progress;

public class CharacterShop : MonoBehaviour
{
    [System.Serializable]
    public class CharacterItem
    {
        public string key;
        public Sprite icon;
        public int price;
        public Button buyButton;
        public Text buyText;
        public Button selectButton;
        public Text selectText;
        public Image characterImage;
        public GameObject selectedMarker;
        public Text priceText;
        public Image startedWeapon;
        public CharacterData characterData;
        public Text CharacterNameText;
    }

    [SerializeField] private RectTransform[] _heroes;
    [SerializeField] private Button _nextButton;

    public List<CharacterItem> characters;
    public Text coinsText;


    private void Start()
    {
        UpdateShopUI();
    }

    private void OnEnable()
    {

        _nextButton.onClick.AddListener(NextHero);
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(NextHero);
    }

    public void BuyCharacter(string characterKey)
    {
        Debug.Log(characterKey);
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

    private void UpdateShopUI()
    {
        coinsText.text = LocalizationManager.Instance.GetTranslation("balace_text")
                        .Replace("{countCoins}", YG2.saves.coins.ToString());

        foreach (var item in characters)
        {
            // »конка и цена
            item.CharacterNameText.text = item.characterData.characterName;
            item.characterImage.sprite = item.icon;
            item.priceText.text = LocalizationManager.Instance.GetTranslation("price_text")
                        .Replace("{price}", item.price.ToString());
            item.selectText.text = LocalizationManager.Instance.GetTranslation("select_button");
            item.buyText.text = LocalizationManager.Instance.GetTranslation("buy_button");
            item.startedWeapon.sprite = item.characterData.startedWeapon.Icon;


            bool unlocked = YG2.saves.unlockedCharacters.Contains(item.key);
            bool selected = item.key == YG2.saves.selectedCharacter;

            item.buyButton.interactable = !unlocked && YG2.saves.coins >= item.price;
            item.buyButton.gameObject.SetActive(!unlocked);
            item.selectButton.gameObject.SetActive(unlocked);
            item.selectedMarker.SetActive(selected);

            item.buyButton.onClick.RemoveAllListeners();
            item.buyButton.onClick.AddListener(() => BuyCharacter(item.key));
            item.selectButton.onClick.RemoveAllListeners();
            item.selectButton.onClick.AddListener(() => SelectCharacter(item.key));
        }


    }

    private void NextHero()
    {
        foreach (var hero in _heroes)
        {
            hero.DOAnchorPos3DX(hero.transform.position.x - 1000, 2).SetEase(Ease.OutQuad);
        }
    }
}
