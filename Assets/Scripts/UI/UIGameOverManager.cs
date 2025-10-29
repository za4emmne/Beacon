using UnityEngine;
using UnityEngine.UI;

public class UIGameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private UIStats _stats;
    [SerializeField] private Button _raisePlayer;

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
        //_stats.gameObject.SetActive(true);
    }

    public void OnDeadScreenActivate()
    {
        int kill = _gameManager.Kill;
        int level = Player.singleton.GetComponent<PlayerLevelManager>().Level;
        string time = _timerLink.GetCurrentTimeText();

        _stats.CurrentStatsUpdate(kill, level, time);
        _gameOverScreen.SetActive(true);
        //_stats.gameObject.SetActive(false);

        if (_gameManager.RaiseCount <= 0)
            _raisePlayer.gameObject.SetActive(false);
    }
}
