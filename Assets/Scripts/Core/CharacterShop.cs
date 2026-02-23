using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class CharacterShop : MonoBehaviour
{
    [System.Serializable]
    public class CharacterItemUI
    {
        public string key;                     // ���� ����� (��������� � CharacterData)
        public CharacterData characterData;    // ������ �����

        [Header("UI")]
        public Image iconImage;
        public Text characterNameText;
        public Image startedWeaponImage;

        public Text priceText;
        public Button buyButton;
        public Text buyText;

        public Button selectButton;
        public Text selectText;

        public GameObject selectedMarker;
    }

    [Header("Hero Scroll / Slider")]
    [SerializeField] private RectTransform[] _heroes;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _lastButton;

    [Header("Characters")]
    public List<CharacterItemUI> characters;

    [Header("Currency UI")]
    public Text coinsText;
    public Button addCoinsButton;   // ������ ���������� ������� (��������, �� �������/�����)

    private int _currentHeroIndex = 0;
    private const float HERO_SHIFT = 1200f;
    private const float HERO_TWEEN_DURATION = 0.3f;

    private void Start()
    {
        InitButtons();
        UpdateShopUI();
    }

    private void OnEnable()
    {
        _nextButton.onClick.AddListener(NextHero);
        _lastButton.onClick.AddListener(LastHero);
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(NextHero);
        _lastButton.onClick.RemoveListener(LastHero);
    }

    private void InitButtons()
    {
        // ������ ���������� �����
        if (addCoinsButton != null)
            addCoinsButton.onClick.AddListener(AddCoinsDebug); // ��� �������� ���� ������ YG2 (�������/�������)

        // ��� ������� ����� ���������� ����������� ���� ���
        foreach (var item in characters)
        {
            var localKey = item.key; // ��������� �����, ����� ��������� �������� ���������

            item.buyButton.onClick.RemoveAllListeners();
            item.buyButton.onClick.AddListener(() => BuyCharacter(localKey));

            item.selectButton.onClick.RemoveAllListeners();
            item.selectButton.onClick.AddListener(() => SelectCharacter(localKey));
        }
    }

    private void UpdateShopUI()
    {
        // ��������� ����� �������
        coinsText.text = LocalizationManager.Instance.GetTranslation("balace_text")
            .Replace("{countCoins}", YG2.saves.coins.ToString());

        foreach (var item in characters)
        {
            bool unlocked = YG2.saves.unlockedCharacters.Contains(item.key);
            bool selected = item.key == YG2.saves.selectedCharacter;

            // ��������� ������� �� CharacterData
            if (item.characterData != null)
            {
                if (item.iconImage != null)
                    item.iconImage.sprite = item.characterData.icon;

                if (item.characterNameText != null)
                    item.characterNameText.text = item.characterData.characterName;

                if (item.startedWeaponImage != null && item.characterData.startedWeapon != null)
                    item.startedWeaponImage.sprite = item.characterData.startedWeapon.Icon;
            }

            // ���� � ����������� ������
            if (item.priceText != null)
            {
                item.priceText.text = LocalizationManager.Instance.GetTranslation("price_text")
                    .Replace("{price}", item.characterData.price.ToString());
            }

            if (item.buyText != null)
                item.buyText.text = LocalizationManager.Instance.GetTranslation("buy_button");

            if (item.selectText != null)
            {
                if (selected)
                    item.selectText.text = LocalizationManager.Instance.GetTranslation("selected_button");
                else
                    item.selectText.text = LocalizationManager.Instance.GetTranslation("select_button");
            }

            // ������ �����������
            int price = item.characterData != null ? item.characterData.price : 0;

            if (item.buyButton != null)
            {
                item.buyButton.gameObject.SetActive(!unlocked);
                item.buyButton.interactable = !unlocked && YG2.saves.coins >= price;
            }

            if (item.selectButton != null)
            {
                item.selectButton.gameObject.SetActive(unlocked);
                item.selectButton.interactable = unlocked && !selected;
            }

            if (item.selectedMarker != null)
                item.selectedMarker.SetActive(selected);

        }
    }

    public void BuyCharacter(string characterKey)
    {
        var item = characters.Find(x => x.key == characterKey);
        if (item == null || item.characterData == null)
            return;

        int price = item.characterData.price;

        if (!YG2.saves.unlockedCharacters.Contains(characterKey) && YG2.saves.coins >= price)
        {
            YG2.saves.coins -= price;
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

    // ������ ���������� ����� (� ���������� ���� ����� YG2 Rewarded Ad, IAP � �.�.)
    private void AddCoinsDebug()
    {
        YG2.RewardedAdvShow("reclama", () =>
        {
            YG2.saves.coins += 100;
            YG2.SaveProgress();
            UpdateShopUI();
        });
    }

    // ������������ ������ � ��������� DOTween
    private void NextHero()
    {
        _currentHeroIndex = (_currentHeroIndex + 1) % _heroes.Length;
        UpdateHeroesPositions();
    }

    private void LastHero()
    {
        _currentHeroIndex = (_currentHeroIndex - 1 + _heroes.Length) % _heroes.Length;
        UpdateHeroesPositions();
    }

    private void UpdateHeroesPositions()
    {
        for (int i = 0; i < _heroes.Length; i++)
        {
            float targetX = (i - _currentHeroIndex) * HERO_SHIFT;
            _heroes[i].DOAnchorPosX(targetX, HERO_TWEEN_DURATION).SetEase(Ease.OutQuad);
        }
    }

}

