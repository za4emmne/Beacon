using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private UIStats _stats;
    [SerializeField] private Button _raisePlayer;
    [SerializeField] private Button _addPriceCoin;

    private void Awake()
    {
        InitDebug.Log($"[INIT][UIGameOverManager] Awake() - this={GetInstanceID()}, scene={SceneManager.GetActiveScene().name}");
    }

    private void Start()
    {
        InitDebug.Log("[INIT][UIGameOverManager] Start()");
        
        if (_raisePlayer != null && GameManager.Instance != null)
            _raisePlayer.onClick.AddListener(GameManager.Instance.OnRaisePlayer);
            
        if (_addPriceCoin != null && GameManager.Instance != null)
            _addPriceCoin.onClick.AddListener(GameManager.Instance.OnGetCoins);
            
        InitDebug.Log("[INIT][UIGameOverManager] Button listeners registered");
    }

    private void OnEnable()
    {
        InitDebug.Log("[INIT][UIGameOverManager] OnEnable()");
    }

    private void OnDisable()
    {
        InitDebug.Log("[INIT][UIGameOverManager] OnDisable()");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnAddCoin -= HidePriseButton;
            InitDebug.Log("[EVENT][UIGameOverManager] Unsubscribed from OnAddCoin");
        }
    }

    private void OnDestroy()
    {
        InitDebug.Log($"[INIT][UIGameOverManager] OnDestroy() - this={GetInstanceID()}");
    }

    public void OnDeadScreenDisactivate()
    {
        InitDebug.Log("[UI][UIGameOverManager] OnDeadScreenDisactivate()");
        
        if (_gameOverScreen != null)
            _gameOverScreen.SetActive(false);
    }

    public void OnDeadScreenActivate()
    {
        InitDebug.Log("[UI][UIGameOverManager] OnDeadScreenActivate() called");
        
        if (GameManager.Instance == null || Player.singleton == null)
        {
            InitDebug.LogError("[UI][UIGameOverManager] GameManager or Player is NULL!");
            return;
        }
        
        int kill = GameManager.Instance.CurrentKill;
        int level = Player.singleton.GetComponent<PlayerLevelManager>().Level;
        string time = Timer.Instance?.GetCurrentTimeText() ?? "00:00";
        int coin = GameManager.Instance.CurrentCoin;

        InitDebug.Log($"[UI][UIGameOverManager] Stats: kill={kill}, level={level}, time={time}, coin={coin}");

        if (_stats != null)
            _stats.CurrentStatsUpdate(kill, level, time, coin);
            
        if (_gameOverScreen != null)
            _gameOverScreen.SetActive(true);
            
        if (_stats != null)
            _stats.gameObject.SetActive(false);

        if (GameManager.Instance.RaiseCount <= 0 && _raisePlayer != null)
            _raisePlayer.gameObject.SetActive(false);

        GameManager.Instance.OnAddCoin += HidePriseButton;
        InitDebug.Log("[EVENT][UIGameOverManager] Subscribed to OnAddCoin");
    }

    private void HidePriseButton(int value)
    {
        InitDebug.Log($"[UI][UIGameOverManager] HidePriseButton({value})");
        
        if (_addPriceCoin != null)
            _addPriceCoin.gameObject.SetActive(false);
    }
}