using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private ButtonManager _buttonManager;
    [SerializeField] private Text _playerTalk;
    [SerializeField] private Button _achive;
    [SerializeField] private Button _start;

    private void Start()
    {
        _achive.onClick.AddListener(ShowAchiveScreen);
        _start.onClick.AddListener(StartGame);
    }

    private void OnEnable()
    {
        _buttonManager.OnShowTooltip += ShowDemo;
        _buttonManager.OnHideTooltip += HideDemo;
    }

    private void OnDisable()
    {
        _buttonManager.OnShowTooltip -= ShowDemo;
        _buttonManager.OnHideTooltip += HideDemo;
    }

    private void ShowDemo()//сделать случайную фразу
    {
        if (_playerTalk.gameObject.activeSelf != true)
        {
            _playerTalk.gameObject.SetActive(true);
            _playerTalk.text = "Нажимай, Шрек!";
        }
        else
        {
            _playerTalk.text = "Нажимай, Шрек!";
        }
    }

    private void HideDemo()
    {
        if (_playerTalk.gameObject.activeSelf)
            _playerTalk.gameObject.SetActive(false);
    }

    private void ShowAchiveScreen()
    {
        Debug.Log("Show");
    }

    public void StartGame()//добавить подписку на события
    {
        SceneManager.LoadScene("Game");
    }
}
