using System;
using UnityEngine;
using YG;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    private int _bestScore;
    private int _bestLevel;
    private int _totalCoins;
    private float _time;


    public int BestScore => _bestScore;
    public int BestLevel => _bestLevel;
    public int TotalCoins => _totalCoins;
    public float Time => _time;

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
        // Добавь другие данные по необходимости
        Debug.Log($"Data loaded: BestScore={_bestScore}, BestLevel={_bestLevel}");
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
}
