using Cinemachine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    public List<CharacterData> Characters = new List<CharacterData>();
    public List<BiomeData> Locations = new List<BiomeData>();

    private int _bestScore;
    private int _bestLevel;
    private int _totalCoins;
    private float _bestTime;
    private float _totalTime;
    private int _totalKill;
    private CharacterData _currentCharacter;
    private BiomeData _currentLocation;


    public int BestScore => _bestScore;
    public int BestLevel => _bestLevel;
    public int TotalCoins => _totalCoins;
    public float BestTime => _bestTime;
    public float TotalTime => _totalTime;
    public int TotalKill => _totalKill;
    public BiomeData CurrentLocation => _currentLocation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        YG2.onGetSDKData += LoadData;
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= LoadData;
    }

    private void Start()
    {
        if (_bestScore == 0 && _bestLevel == 0 && _totalCoins == 0 && YG2.saves != null)
        {
            LoadData();
        }
    }

    public void LoadData()
    {
        _bestScore = YG2.saves.bestScore;
        _bestLevel = YG2.saves.bestLevel;
        _totalCoins = YG2.saves.coins;
        _totalKill = YG2.saves.totalKill;
        _bestTime = YG2.saves.bestTime;
        _totalTime = YG2.saves.totalTime;

        InitializeDefaultData();

        Debug.Log($"Data loaded: BestScore={_bestScore}, BestLevel={_bestLevel}");
    }

    private void InitializeDefaultData()
    {
        bool isFirstLaunch = YG2.saves.unlockedCharacters == null || YG2.saves.unlockedCharacters.Count == 0;

        if (isFirstLaunch)
        {
            CharacterData defaultCharacter = Characters.Find(c => c.isDefault);
            if (defaultCharacter != null)
            {
                YG2.saves.unlockedCharacters = new List<string> { defaultCharacter.characterKey };
                YG2.saves.selectedCharacter = defaultCharacter.characterKey;
                YG2.SaveProgress();
                Debug.Log($"First launch: unlocked default character '{defaultCharacter.characterKey}'");
            }
        }

        InitializeDefaultLocation();
        _currentLocation = GetCurrentSelectedLocation();
    }

    public CharacterData GetCharacter(string key)
    {
        return Characters.Find(c => c.characterKey == key);
    }

    public CharacterData CurrentCharacter => GetCharacter(YG2.saves.selectedCharacter);

    public void UpdateTotalKill(int currentKill)
    {
        _totalKill += currentKill;
        YG2.saves.totalKill = _totalKill;
        YG2.SaveProgress();
    }

    public void UpdateBestScore(int newScore)
    {
        if (newScore > _bestScore)
        {
            _bestScore = newScore;
            YG2.saves.bestScore = _bestScore;
            YG2.SaveProgress();
        }
    }

    public void UpdateBestTime(float time)
    {
        if (_bestTime < time)
        {
            _bestTime = time;
            YG2.saves.bestTime = _bestTime;
            YG2.SaveProgress(); 
        }
    }

    public void UpdateTotalTime(float time)
    {
        _totalTime += time;
        YG2.saves.totalTime = _totalTime;
        YG2.SaveProgress();
    }

    public void UpdateBestLevel(int newLevel)
    {
        if (newLevel > _bestLevel)
        {
            _bestLevel = newLevel;
            YG2.saves.bestLevel = _bestLevel;
            YG2.SaveProgress();
        }
    }

    public void AddCoins(int amount)
    {
        _totalCoins += amount;
        YG2.saves.coins = _totalCoins;
        YG2.SaveProgress();
    }

    public BiomeData GetLocation(string locationId)
    {
        return Locations.Find(l => l.biomeId == locationId);
    }

    public bool IsLocationUnlocked(string locationId)
    {
        return YG2.saves.unlockedLocations.Contains(locationId);
    }

    public bool CanAffordLocation(BiomeData location)
    {
        return _totalCoins >= location.price;
    }

    public bool PurchaseLocation(BiomeData location)
    {
        if (IsLocationUnlocked(location.biomeId))
            return false;

        if (!CanAffordLocation(location))
            return false;

        _totalCoins -= location.price;
        YG2.saves.coins = _totalCoins;
        YG2.saves.unlockedLocations.Add(location.biomeId);
        YG2.SaveProgress();

        Debug.Log($"Purchased location: {location.displayName}");
        return true;
    }

    public void SelectLocation(BiomeData location)
    {
        if (!IsLocationUnlocked(location.biomeId))
            return;

        YG2.saves.selectedLocation = location.biomeId;
        _currentLocation = location;
        YG2.SaveProgress();

        Debug.Log($"Selected location: {location.displayName}");
    }

    public BiomeData GetCurrentSelectedLocation()
    {
        if (!string.IsNullOrEmpty(YG2.saves.selectedLocation))
        {
            BiomeData loc = GetLocation(YG2.saves.selectedLocation);
            if (loc != null) return loc;
        }

        return GetDefaultLocation();
    }

    public BiomeData GetDefaultLocation()
    {
        return Locations.Find(l => l.isDefault);
    }

    private void InitializeDefaultLocation()
    {
        bool isFirstLaunch = YG2.saves.unlockedLocations == null || YG2.saves.unlockedLocations.Count == 0;

        if (isFirstLaunch)
        {
            BiomeData defaultLocation = Locations.Find(l => l.isDefault);
            if (defaultLocation != null)
            {
                YG2.saves.unlockedLocations = new List<string> { defaultLocation.biomeId };
                YG2.saves.selectedLocation = defaultLocation.biomeId;
                YG2.SaveProgress();
                Debug.Log($"First launch: unlocked default location '{defaultLocation.biomeId}'");
            }
        }
    }

    public void ResetProgress()
    {
        _bestScore = 0;
        _bestLevel = 0;
        _totalCoins = 0;
        _bestTime = 0f;
        _totalTime = 0f;
        _totalKill = 0;

        YG2.saves.bestScore = 0;
        YG2.saves.bestLevel = 0;
        YG2.saves.coins = 0;
        YG2.saves.bestTime = 0f;
        YG2.saves.totalTime = 0f;
        YG2.saves.totalKill = 0;

        CharacterData defaultCharacter = Characters.Find(c => c.isDefault);
        if (defaultCharacter != null)
        {
            YG2.saves.unlockedCharacters = new List<string> { defaultCharacter.characterKey };
            YG2.saves.selectedCharacter = defaultCharacter.characterKey;
        }
        else
        {
            YG2.saves.unlockedCharacters = new List<string>();
            YG2.saves.selectedCharacter = "";
        }

        BiomeData defaultLocation = Locations.Find(l => l.isDefault);
        if (defaultLocation != null)
        {
            YG2.saves.unlockedLocations = new List<string> { defaultLocation.biomeId };
            YG2.saves.selectedLocation = defaultLocation.biomeId;
        }
        else
        {
            YG2.saves.unlockedLocations = new List<string>();
            YG2.saves.selectedLocation = "";
        }

        YG2.SaveProgress();
        Debug.Log("Progress reset!");
    }
}
