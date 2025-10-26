using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIButtonSound : MonoBehaviour
{
    private Button _button;
    private ISoundManager _soundManager;

    [Inject]
    private void Construct(ISoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => _soundManager.PlayClick());
    }
}
