using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationShopUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private GameObject _locationCardsContainer;
    [SerializeField] private GameObject _locationCardPrefab;
    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;

    [Header("Settings")]
    [SerializeField] private int _maxVisibleCards = 4;
    [SerializeField] private float _cardSpacing = 20f;

    private List<LocationCardUI> _locationCards = new List<LocationCardUI>();
    private List<BiomeData> _allLocations;
    private HorizontalLayoutGroup _layoutGroup;

    private void Start()
    {
        if (_prevButton != null)
            _prevButton.onClick.AddListener(ScrollLeft);

        if (_nextButton != null)
            _nextButton.onClick.AddListener(ScrollRight);

        _layoutGroup = _locationCardsContainer.GetComponent<HorizontalLayoutGroup>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        RefreshCards();
    }

    public void RefreshCards()
    {
        Debug.Log("[LocationShopUI] RefreshCards called");
        
        if (GameDataManager.Instance == null)
        {
            Debug.LogError("[LocationShopUI] GameDataManager.Instance is null!");
            return;
        }
        
        _allLocations = GameDataManager.Instance.Locations;
        
        Debug.Log($"[LocationShopUI] Locations count: {_allLocations?.Count ?? 0}");
        
        if (_allLocations == null || _allLocations.Count == 0)
        {
            Debug.LogWarning("[LocationShopUI] No locations found in GameDataManager!");
            return;
        }

        if (_locationCardPrefab == null)
        {
            Debug.LogError("[LocationShopUI] _locationCardPrefab is null!");
            return;
        }

        if (_locationCardsContainer == null)
        {
            Debug.LogError("[LocationShopUI] _locationCardsContainer is null!");
            return;
        }

        foreach (var card in _locationCards)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        _locationCards.Clear();

        var uniqueLocations = new HashSet<string>();
        foreach (var location in _allLocations)
        {
            if (location != null && uniqueLocations.Add(location.biomeId))
            {
                CreateLocationCard(location);
            }
        }

        Debug.Log($"[LocationShopUI] Created {_locationCards.Count} unique location cards");
        
        StartCoroutine(RefreshLayout());
        UpdateNavigationButtons();
    }
    
    private System.Collections.IEnumerator RefreshLayout()
    {
        yield return new WaitForEndOfFrame();
        
        if (_locationCardsContainer != null && _scrollRect != null)
        {
            HorizontalLayoutGroup layoutGroup = _locationCardsContainer.GetComponent<HorizontalLayoutGroup>();
            if (layoutGroup == null)
            {
                layoutGroup = _locationCardsContainer.AddComponent<HorizontalLayoutGroup>();
            }
            layoutGroup.spacing = 20f;
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = false;
            
            ContentSizeFitter sizeFitter = _locationCardsContainer.GetComponent<ContentSizeFitter>();
            if (sizeFitter == null)
            {
                sizeFitter = _locationCardsContainer.AddComponent<ContentSizeFitter>();
            }
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            
            RectTransform contentRect = _locationCardsContainer.GetComponent<RectTransform>();
            RectTransform viewportRect = _scrollRect.viewport.GetComponent<RectTransform>();
            
            float viewWidth = viewportRect.rect.width;
            float viewHeight = viewportRect.rect.height;
            float cardWidth = viewWidth * 0.7f;
            float cardHeight = 0;
            
            if (_locationCards.Count > 0 && _locationCards[0] != null)
            {
                cardHeight = _locationCards[0].GetComponent<RectTransform>().rect.height;
            }
            if (cardHeight == 0) cardHeight = 350f;
            
            float spacing = 20f;
            float totalWidth = (_allLocations.Count * cardWidth) + ((_allLocations.Count - 1) * spacing);
            
            contentRect.anchorMin = new Vector2(0.5f, 0.5f);
            contentRect.anchorMax = new Vector2(0.5f, 0.5f);
            contentRect.pivot = new Vector2(0.5f, 0.5f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(totalWidth, cardHeight);
            
            foreach (var card in _locationCards)
            {
                if (card != null)
                {
                    RectTransform cardRect = card.GetComponent<RectTransform>();
                    cardRect.anchorMin = new Vector2(0.5f, 0.5f);
                    cardRect.anchorMax = new Vector2(0.5f, 0.5f);
                    cardRect.pivot = new Vector2(0.5f, 0.5f);
                    cardRect.sizeDelta = new Vector2(cardWidth, cardHeight);
                    cardRect.anchoredPosition = Vector2.zero;
                }
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
            
            PositionNavigationButtons(viewWidth, viewHeight, cardWidth);
            
            Debug.Log($"[LocationShopUI] Content: {totalWidth}x{cardHeight}, card: {cardWidth}x{cardHeight}, view: {viewWidth}x{viewHeight}");
        }
    }
    
    private void PositionNavigationButtons(float viewWidth, float viewHeight, float cardWidth)
    {
        float buttonOffset = cardWidth / 2 + 50f;
        float buttonY = 0;
        
        if (_prevButton != null)
        {
            RectTransform btnRect = _prevButton.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.5f);
            btnRect.anchorMax = new Vector2(0.5f, 0.5f);
            btnRect.pivot = new Vector2(0.5f, 0.5f);
            btnRect.anchoredPosition = new Vector2(-buttonOffset, buttonY);
            btnRect.sizeDelta = new Vector2(60, 60);
        }
        
        if (_nextButton != null)
        {
            RectTransform btnRect = _nextButton.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.5f, 0.5f);
            btnRect.anchorMax = new Vector2(0.5f, 0.5f);
            btnRect.pivot = new Vector2(0.5f, 0.5f);
            btnRect.anchoredPosition = new Vector2(buttonOffset, buttonY);
            btnRect.sizeDelta = new Vector2(60, 60);
        }
    }

    private void CreateLocationCard(BiomeData location)
    {
        GameObject cardObj = Instantiate(_locationCardPrefab, _locationCardsContainer.transform);
        LocationCardUI cardUI = cardObj.GetComponent<LocationCardUI>();

        if (cardUI != null)
        {
            bool isUnlocked = GameDataManager.Instance.IsLocationUnlocked(location.biomeId);
            bool isSelected = GameDataManager.Instance.CurrentLocation != null && 
                            GameDataManager.Instance.CurrentLocation.biomeId == location.biomeId;
            bool canAfford = GameDataManager.Instance.CanAffordLocation(location);

            cardUI.Initialize(location, isUnlocked, isSelected, canAfford);
            _locationCards.Add(cardUI);
        }
    }

    private void ScrollLeft()
    {
        if (_scrollRect != null && _scrollRect.content != null)
        {
            float viewWidth = _scrollRect.viewport.rect.width;
            float cardWidth = viewWidth;
            float scrollAmount = cardWidth * 0.8f;
            
            float currentPos = _scrollRect.horizontalNormalizedPosition;
            float contentWidth = _scrollRect.content.rect.width;
            float normalizedScroll = scrollAmount / contentWidth;
            
            _scrollRect.horizontalNormalizedPosition = Mathf.Max(0, currentPos - normalizedScroll);
        }
    }

    private void ScrollRight()
    {
        if (_scrollRect != null && _scrollRect.content != null)
        {
            float viewWidth = _scrollRect.viewport.rect.width;
            float cardWidth = viewWidth;
            float scrollAmount = cardWidth * 0.8f;
            
            float currentPos = _scrollRect.horizontalNormalizedPosition;
            float contentWidth = _scrollRect.content.rect.width;
            float normalizedScroll = scrollAmount / contentWidth;
            
            _scrollRect.horizontalNormalizedPosition = Mathf.Min(1, currentPos + normalizedScroll);
        }
    }

    private void UpdateNavigationButtons()
    {
        bool canScroll = _locationCards.Count > 1;
        
        if (_prevButton != null)
            _prevButton.gameObject.SetActive(canScroll);

        if (_nextButton != null)
            _nextButton.gameObject.SetActive(canScroll);
            
        if (_scrollRect != null)
        {
            _scrollRect.enabled = canScroll;
            if (!canScroll && _locationCards.Count == 1)
            {
                _locationCards[0].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
    }
}
