using Cinemachine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }
    public static Action OnDataReset;

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
    public CharacterData CurrentCharacter => _currentCharacter;

    private void Awake()
    {
        InitDebug.Log($"[INIT][GameDataManager] Awake() - Instance={Instance?.GetInstanceID()}, this={GetInstanceID()}, scene={SceneManager.GetActiveScene().name}");
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitDebug.Log("[INIT][GameDataManager] Set as Instance, DontDestroyOnLoad=true");
            LoadData();
        }
        else
        {
            InitDebug.LogWarning("[INIT][GameDataManager] Duplicate! Destroying this");
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        InitDebug.Log("[INIT][GameDataManager] OnEnable()");
        YG2.onGetSDKData += LoadData;
        InitDebug.Log("[EVENT][GameDataManager] Subscribed to onGetSDKData");
    }

    private void OnDisable()
    {
        InitDebug.Log("[INIT][GameDataManager] OnDisable()");
        YG2.onGetSDKData -= LoadData;
        InitDebug.Log("[EVENT][GameDataManager] Unsubscribed from onGetSDKData");
    }

    private void OnDestroy()
    {
        InitDebug.Log($"[INIT][GameDataManager] OnDestroy() - Instance={Instance?.GetInstanceID()}, this={GetInstanceID()}");
        
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        InitDebug.Log($"[INIT][GameDataManager] Start() - bestScore={_bestScore}, bestLevel={_bestLevel}, totalCoins={_totalCoins}");
        
        if (_bestScore == 0 && _bestLevel == 0 && _totalCoins == 0 && YG2.saves != null)
        {
            LoadData();
        }
    }

    public void LoadData()
    {
        InitDebug.Log("[INIT][GameDataManager] LoadData() called");
        
        _bestScore = YG2.saves.bestScore;
        _bestLevel = YG2.saves.bestLevel;
        _totalCoins = YG2.saves.coins;
        _totalKill = YG2.saves.totalKill;
        _bestTime = YG2.saves.bestTime;
        _totalTime = YG2.saves.totalTime;

        InitializeDefaultData();

        InitDebug.Log($"[INIT][GameDataManager] Data loaded: BestScore={_bestScore}, BestLevel={_bestLevel}, TotalCoins={_totalCoins}");
    }

    public void ResetRunData()
    {
        InitDebug.Log("[INIT][GameDataManager] ResetRunData() called");
        
        _currentCharacter = null;
        _currentLocation = null;
        
        OnDataReset?.Invoke();
        InitDebug.Log("[EVENT][GameDataManager] OnDataReset invoked");
    }

    public void SetCurrentCharacter(CharacterData character)
    {
        _currentCharacter = character;
        InitDebug.Log($"[INIT][GameDataManager] SetCurrentCharacter: {character?.characterKey}");
    }

    public void SetLocation(BiomeData location)
    {
        _currentLocation = location;
        InitDebug.Log($"[INIT][GameDataManager] SetLocation: {location?.name}");
    }

    public CharacterData GetCurrentCharacter()
    {
        return _currentCharacter;
    }

    public void AddCoins(int amount)
    {
        _totalCoins += amount;
        YG2.saves.coins = _totalCoins;
        YG2.SaveProgress();
    }

    public void UpdateBestScore(int score)
    {
        if (score > _bestScore)
        {
            _bestScore = score;
            YG2.saves.bestScore = _bestScore;
            YG2.SaveProgress();
        }
    }

    public void UpdateBestLevel(int level)
    {
        if (level > _bestLevel)
        {
            _bestLevel = level;
            YG2.saves.bestLevel = _bestLevel;
            YG2.SaveProgress();
        }
    }

    public void UpdateTotalKill(int kills)
    {
        _totalKill += kills;
        YG2.saves.totalKill = _totalKill;
        YG2.SaveProgress();
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
            }
        }

        CharacterData selectedChar = Characters.Find(c => c.characterKey == YG2.saves.selectedCharacter);
        if (selectedChar == null)
            selectedChar = Characters.Find(c => c.isDefault);

        if (selectedChar != null)
            _currentCharacter = selectedChar;

    }

    public void UpdateTotalTime(float time)
    {
        _totalTime += time;
        YG2.saves.totalTime = _totalTime;
        YG2.SaveProgress();
    }

    public void UpdateBestTime(float time)
    {
        if (time > _bestTime)
        {
            _bestTime = time;
            YG2.saves.bestTime = _bestTime;
            YG2.SaveProgress();
        }
    }

public bool IsLocationUnlocked(BiomeData location)
    {
        if (location == null) return false;
        string biomeId = location.biomeId;
        return YG2.saves.unlockedLocations != null && YG2.saves.unlockedLocations.Contains(biomeId);
    }
    
    public bool IsLocationUnlocked(string biomeId)
    {
        if (string.IsNullOrEmpty(biomeId)) return false;
        return YG2.saves.unlockedLocations != null && YG2.saves.unlockedLocations.Contains(biomeId);
    }

    public void SelectLocation(BiomeData location)
    {
        if (location == null || !IsLocationUnlocked(location.biomeId)) return;
        _currentLocation = location;
        YG2.saves.selectedLocation = location.biomeId;
        YG2.SaveProgress();
    }
    
    public void SelectLocation(string biomeId)
    {
        if (string.IsNullOrEmpty(biomeId) || !IsLocationUnlocked(biomeId)) return;
        BiomeData location = Locations.Find(l => l.biomeId == biomeId);
        if (location != null)
        {
            _currentLocation = location;
            YG2.saves.selectedLocation = biomeId;
            YG2.SaveProgress();
        }
    }

    public bool PurchaseLocation(BiomeData location)
    {
        if (location == null || IsLocationUnlocked(location.biomeId)) return false;
        
        int price = location.price;
        if (_totalCoins < price) return false;
        
        _totalCoins -= price;
        YG2.saves.coins = _totalCoins;
        
        if (YG2.saves.unlockedLocations == null)
            YG2.saves.unlockedLocations = new List<string>();
        YG2.saves.unlockedLocations.Add(location.biomeId);
        
        YG2.SaveProgress();
        return true;
    }
    
    public bool PurchaseLocation(string biomeId)
    {
        if (string.IsNullOrEmpty(biomeId) || IsLocationUnlocked(biomeId)) return false;
        
        BiomeData location = Locations.Find(l => l.biomeId == biomeId);
        if (location == null) return false;
        
        int price = location.price;
        if (_totalCoins < price) return false;
        
        _totalCoins -= price;
        YG2.saves.coins = _totalCoins;
        
        if (YG2.saves.unlockedLocations == null)
            YG2.saves.unlockedLocations = new List<string>();
        YG2.saves.unlockedLocations.Add(biomeId);
        
        YG2.SaveProgress();
        return true;
    }

    public bool CanAffordLocation(BiomeData location)
    {
        if (location == null) return false;
        return _totalCoins >= location.price;
    }
    
    public bool CanAffordLocation(string biomeId)
    {
        if (string.IsNullOrEmpty(biomeId)) return false;
        BiomeData location = Locations.Find(l => l.biomeId == biomeId);
        if (location == null) return false;
        return _totalCoins >= location.price;
    }
}