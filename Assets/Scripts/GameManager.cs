using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemiesGenerator _enemyManager;

    private UIManager _uiManager;
    private PlayerProgress _progress;
    private int _score;

    public int Score => _score;

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
        _progress = Player.singleton.GetComponent<PlayerProgress>();
    }

    private void Start()
    {
        _score = 0;
        _enemyManager.OnStartGenerator();
    }

    private void OnEnable()
    {
        _enemyManager.oneKill += ChangeScore;
        _progress.LevelUp += FreezeTime;
    }

    private void OnDisable()
    {
        _enemyManager.oneKill -= ChangeScore;
        _progress.LevelUp -= FreezeTime;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("islandScene");
        Time.timeScale = 1f;
    }

    private void ChangeScore()
    {
        _score++;
        _uiManager.ChangeScore(_score);
    }

    private void FreezeTime()
    {
        Time.timeScale = 0f;
    }
}
