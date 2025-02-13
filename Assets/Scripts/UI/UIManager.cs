using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject[] _gameOverScreen;
    [SerializeField] private GameObject[] _levelUpScreen;
    [SerializeField] private GameObject _levelUp;

    private PlayerProgress _player;

    private void Awake()
    {
        _player = Player.singleton.GetComponent<PlayerProgress>();
    }
    private void Start()
    {
        ScreenManage(_gameOverScreen, false);
        ScreenManage(_levelUpScreen, false);
        ScreenManage(_levelUpScreen, false);
    }

    private void OnEnable()
    {
        _player.LevelUp += OnLevelUpScreen;
    }

    private void OnDisable()
    {
        _player.LevelUp -= OnLevelUpScreen;
    }

    public void ChangeScore(int Score)
    {
        _scoreText.text = "—чет: " + Score;
    }

    public void OnLevelUpScreen()
    {
        ScreenManage(_levelUpScreen, true);
    }

    public void OnDeadScreen()
    {
        ScreenManage(_gameOverScreen, true);
    }

    private void ScreenManage(GameObject[] gameObjects, bool status)
    {
        foreach (var ui in gameObjects)
        {
            ui.SetActive(status);
        }
    }
}
