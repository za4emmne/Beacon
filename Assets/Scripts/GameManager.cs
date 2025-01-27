using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;

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

    private void ChangeScore()
    {
        _score++;
        _uiManager.ChangeScore(_score);
    }
}
