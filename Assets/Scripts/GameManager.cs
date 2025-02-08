using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private StarGenerator _starGenerator;

    private UIManager _uiManager;
    private int _score;

    public int Score => _score;

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        _score = 0;
    }

    private void OnEnable()
    {
        _enemyManager.oneKill += ChangeScore;
    }

    private void OnDisable()
    {
        _enemyManager.oneKill -= ChangeScore;
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
}
