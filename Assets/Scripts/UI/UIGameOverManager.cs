using UnityEngine;
using UnityEngine.UI;

public class UIGameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private UIStats _stats;
    [SerializeField] private Button _raisePlayer;
    [SerializeField] private Button _addPriceCoin;

    private void Start()
    {
        _raisePlayer.onClick.AddListener(GameManager.Instance.OnRaisePlayer);
        _addPriceCoin.onClick.AddListener(GameManager.Instance.OnGetCoins);
    }

    public void OnDeadScreenDisactivate()
    {
        _gameOverScreen.SetActive(false);
        //_stats.gameObject.SetActive(true);
    }

    public void OnDeadScreenActivate()
    {
        int kill = GameManager.Instance.CurrentKill;
        int level = Player.singleton.GetComponent<PlayerLevelManager>().Level;
        string time = Timer.Instance.GetCurrentTimeText();
        int coin = GameManager.Instance.CurrentCoin;

        _stats.CurrentStatsUpdate(kill, level, time, coin);
        _gameOverScreen.SetActive(true);
        _stats.gameObject.SetActive(false);

        if (GameManager.Instance.RaiseCount <= 0)
            _raisePlayer.gameObject.SetActive(false);

        GameManager.Instance.OnAddCoin += HidePriseButton;

    }

    private void HidePriseButton(int value)
    {
        _addPriceCoin.gameObject.SetActive(false);
    }
}
