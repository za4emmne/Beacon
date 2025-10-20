using UnityEngine;
using UnityEngine.UI;

public class UIGameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _stats;
    [SerializeField] private Button _raisePlayer;

    [Header("Cтатистика")]
    [SerializeField] private Text _kill;
    [SerializeField] private Text _timer;
    [SerializeField] private Text _coin;
    [SerializeField] private Text _level;

    private GameManager _gameManager;
    private Timer _timerLink;

    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
        _timerLink = GetComponent<Timer>();
    }

    private void Start()
    {
        _raisePlayer.onClick.AddListener(_gameManager.OnRaisePlayer);
    }

    public void OnDeadScreenDisactivate()
    {
        _gameOverScreen.SetActive(false);
        _stats.SetActive(true);
    }

    public void OnDeadScreenActivate()
    {
        int kill = _gameManager.Kill;
        int level = Player.singleton.GetComponent<PlayerLevelManager>().Level;
        string time = _timerLink.GetCurrentTimeText();

        _gameOverScreen.SetActive(true);
        _stats.SetActive(false);
        _kill.text = "Убито: " + kill; 
        _level.text = "Уровень: " + level;  
        _timer.text = "Время игры: " + time;

        if (_gameManager.RaiseCount <= 0)
            _raisePlayer.gameObject.SetActive(false);
    }
}
