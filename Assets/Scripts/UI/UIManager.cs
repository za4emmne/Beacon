using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject[] _gameOverScreen;

    private void Start()
    {
        DeadScreenManage(false);
    }

    public void ChangeScore(int Score)
    {
        _scoreText.text = "—чет: " + Score;
    }

    public void OnDeadScreen()
    {
        DeadScreenManage(true);
    }

    private void DeadScreenManage(bool status)
    {
        foreach (var ui in _gameOverScreen)
        {
            ui.SetActive(status);
        }
    }
}
