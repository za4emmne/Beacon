using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _deadScreen;

    private void Start()
    {
        _deadScreen.SetActive(false);
    }

    public void ChangeScore(int Score)
    {
        _scoreText.text = "����: " + Score;
    }

    public void OnDeadScreen()
    {
        _deadScreen.SetActive(true);
    }
}
