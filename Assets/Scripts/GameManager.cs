using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;
using YG;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemiesGenerator _enemyManager;
    [SerializeField] private StarGenerator _starGenerator;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private AudioClip _levelUpAudio;

    private UIManager _uiManager;
    private AudioSource _audioSource;
    private PlayerLevelManager _progress;
    private PlayerHealth _playerHealth;

    private int _score;
    private int _highScore;
    private int _coins;
    private int _raiseCount;

    public int Score => _score;
    public int HighScore => _highScore;
    public int Coins => _coins;
    public int RaiseCount => _raiseCount;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GetComponent<UIManager>();
        _progress = Player.singleton.GetComponent<PlayerLevelManager>();
        _playerHealth = Player.singleton.GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        _raiseCount = 1;
        _score = 0;
    }

    private void OnEnable()
    {
        YG2.onGetSDKData += GetLoad;
        _enemyManager.OneKill += ChangeScore;
        _progress.LevelUp += _uiManager.ChangeLevel;
        _progress.LevelUp += LevelUpAudioPlay;
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= GetLoad;
        _enemyManager.OneKill -= ChangeScore;
        _progress.LevelUp -= _uiManager.ChangeLevel;
        _progress.LevelUp -= LevelUpAudioPlay;
    }

    public void OnRaisePlayer()
    {
        if (_raiseCount > 0)
            _raiseCount--;

        _playerHealth.Raise();
        _uiManager.OnDeadScreenDisactivate();
        Time.timeScale = 1f;
    }

    private void GetLoad()
    {
        _highScore = YG2.saves.bestScore;

        if (_progress != null)
        {
            _progress.GetBestLevel();
        }
        else
        {
            _progress = Player.singleton.GetComponent<PlayerLevelManager>();
            _progress.GetBestLevel();
        }

    }

    private void ChangeScore()
    {
        _score++;
        _uiManager.ChangeScore(_score);

        if (_score >= _highScore)
        {
            _highScore -= _score;
            YG2.saves.bestScore = _highScore;
            YG2.SaveProgress();
        }
    }

    private void LevelUpAudioPlay()
    {
        _audioSource.PlayOneShot(_levelUpAudio);
    }
}
