using UnityEngine;
using UnityEngine.UI;

public class LocationCardUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private Text _priceText;
    [SerializeField] private Button _actionButton;
    [SerializeField] private Text _actionButtonText;
    [SerializeField] private GameObject _selectedMarker;

    [Header("Card Settings")]
    [SerializeField] private float _cardHeight = 350f;

    private BiomeData _locationData;

    public void Initialize(BiomeData location, bool isUnlocked, bool isSelected, bool canAfford)
    {
        _locationData = location;

        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, _cardHeight);

        if (_iconImage != null && location.icon != null)
            _iconImage.sprite = location.icon;

        if (_nameText != null)
            _nameText.text = LocalizationManager.Instance.GetTranslation(location.displayName);

        if (_descriptionText != null)
            _descriptionText.text = LocalizationManager.Instance.GetTranslation(location.description);

        if (_selectedMarker != null)
            _selectedMarker.SetActive(isSelected);

        if (_actionButton != null)
        {
            _actionButton.onClick.RemoveAllListeners();
            _actionButton.onClick.AddListener(OnActionButtonClick);
        }

        UpdateButtonState(isUnlocked, isSelected, canAfford);
    }

    private void UpdateButtonState(bool isUnlocked, bool isSelected, bool canAfford)
    {
        if (_actionButton == null) return;

        Debug.Log($"[LocationCardUI] {(_locationData?.displayName)}: isUnlocked={isUnlocked}, isSelected={isSelected}, canAfford={canAfford}, price={_locationData?.price}, coins={GameDataManager.Instance?.TotalCoins}");

        if (isSelected)
        {
            _actionButton.gameObject.SetActive(true);
            _actionButton.interactable = false;
            if (_actionButtonText != null)
                _actionButtonText.text = LocalizationManager.Instance.GetTranslation("selected_button");
        }
        else if (isUnlocked)
        {
            _actionButton.gameObject.SetActive(true);
            _actionButton.interactable = true;
            if (_actionButtonText != null)
                _actionButtonText.text = LocalizationManager.Instance.GetTranslation("select_button");
        }
        else
        {
            _actionButton.gameObject.SetActive(true);
            _actionButton.interactable = canAfford;
            if (_priceText != null)
                _priceText.text = _locationData.price.ToString();
            if (_actionButtonText != null)
                _actionButtonText.text = LocalizationManager.Instance.GetTranslation("buy_button");
        }
    }

    public void OnActionButtonClick()
    {
        if (_locationData == null) return;

        var gameData = GameDataManager.Instance;
        bool isUnlocked = gameData.IsLocationUnlocked(_locationData.biomeId);

        if (isUnlocked)
        {
            gameData.SelectLocation(_locationData);
        }
        else
        {
            if (gameData.PurchaseLocation(_locationData))
            {
                RefreshCard();
            }
        }
    }

    private void RefreshCard()
    {
        bool isUnlocked = GameDataManager.Instance.IsLocationUnlocked(_locationData.biomeId);
        bool isSelected = GameDataManager.Instance.CurrentLocation != null && 
                        GameDataManager.Instance.CurrentLocation.biomeId == _locationData.biomeId;
        bool canAfford = GameDataManager.Instance.CanAffordLocation(_locationData);

        Initialize(_locationData, isUnlocked, isSelected, canAfford);
    }
}
