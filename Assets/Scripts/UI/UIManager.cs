using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _levelText;
    [SerializeField] private GameObject[] _gameOverScreen;
    [SerializeField] private PlayerProgress _player;

    private void Start()
    {
        ScreenManage(_gameOverScreen, false);
    }

    public void ChangeScore(int Score)
    {
        _scoreText.text = "—чет: " + Score;
    }

    public void ChangeLevel()
    {
        _levelText.text = "”ровень: " + _player.Level;
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
