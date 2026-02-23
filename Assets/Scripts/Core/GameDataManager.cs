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

    private int _bestScore;
    private int _bestLevel;
    private int _totalCoins;
    private float _bestTime;
    private float _totalTime;
    private int _totalKill;
    private CharacterData _currentCharacter;


    public int BestScore => _bestScore;
    public int BestLevel => _bestLevel;
    public int TotalCoins => _totalCoins;
    public float BestTime => _bestTime;
    public float TotalTime => _totalTime;
    public int TotalKill => _totalKill;

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

        YG2.SaveProgress();
        Debug.Log("Progress reset!");
    }
}
