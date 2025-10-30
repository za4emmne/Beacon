using UnityEngine;
using UnityEngine.UI;

public class UIGameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private UIStats _stats;
    [SerializeField] private Button _raisePlayer;

    private Timer _timerLink;

    private void Awake()
    {
        _timerLink = GetComponent<Timer>();
    }

    private void Start()
    {
        _raisePlayer.onClick.AddListener(GameManager.Instance.OnRaisePlayer);
    }

    public void OnDeadScreenDisactivate()
    {
        _gameOverScreen.SetActive(false);
        //_stats.gameObject.SetActive(true);
    }

    public void OnDeadScreenActivate()
    {
        int kill = GameManager.Instance.Kill;
        int level = Player.singleton.GetComponent<PlayerLevelManager>().Level;
        string time = _timerLink.GetCurrentTimeText();

        _stats.CurrentStatsUpdate(kill, level, time);
        _gameOverScreen.SetActive(true);
        //_stats.gameObject.SetActive(false);

        if (GameManager.Instance.RaiseCount <= 0)
            _raisePlayer.gameObject.SetActive(false);
    }
}
