using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemiesGenerator _enemyManager;
    [SerializeField] private StarGenerator _starGenerator;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private AudioClip _levelUpAudio;

    private UIManager _uiManager;
    private AudioSource _audioSource;
    private PlayerProgress _progress;
    private int _score;

    public int Score => _score;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GetComponent<UIManager>();
        _progress = Player.singleton.GetComponent<PlayerProgress>();
    }

    private void Start()
    {
        _score = 0;
        //_enemyManager.OnStartGenerator();
    }

    private void OnEnable()
    {
        _enemyManager.OneKill += ChangeScore;
        _progress.LevelUp += _uiManager.ChangeLevel;
        _progress.LevelUp += LevelUpAudioPlay;
    }

    private void OnDisable()
    {
        _enemyManager.OneKill -= ChangeScore;
        _progress.LevelUp -= _uiManager.ChangeLevel;
        _progress.LevelUp -= LevelUpAudioPlay;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("islandScene");
    }

    private void ChangeScore()
    {
        _score++;
        _uiManager.ChangeScore(_score);
    }

    private void LevelUpAudioPlay()
    {
        _audioSource.PlayOneShot(_levelUpAudio);
    }
}
