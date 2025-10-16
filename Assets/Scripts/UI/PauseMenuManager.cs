using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private RectTransform _pauseUI;
    [SerializeField] private AudioMixerGroup _audioMixer;
    [SerializeField] private UnityEngine.UI.Button _continueButton;
    [SerializeField] private UnityEngine.UI.Slider _sliderMusic;
    [SerializeField] private UnityEngine.UI.Toggle _toggleMusic;
    [SerializeField] private UnityEngine.UI.Button _menu;

    [Header("Настройки анимации")]
    [SerializeField] private Vector2 _hiddenPosition;
    [SerializeField] private Vector2 _shownPosition;
    [SerializeField] private float _animationDuration;

    private UIManager _manager;

    private void Awake()
    {
        _manager = GetComponent<UIManager>();
    }

    private void Start()
    {
        _pauseScreen.SetActive(false);
        _pauseUI.anchoredPosition = _hiddenPosition;
        _pauseUI.gameObject.SetActive(false);
        _continueButton.onClick.AddListener(ContinueGame);
        _menu.onClick.AddListener(_manager.LoadMenuScene);
    }

    public void Pause()
    {

        _pauseScreen.SetActive(true);
        _pauseUI.gameObject.SetActive(true);
        _pauseUI.DOAnchorPos(_shownPosition, _animationDuration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Time.timeScale = 0f;
        });
    }

    public void ToggleMusic(bool enabled)
    {
        if (_toggleMusic.isOn)
            _audioMixer.audioMixer.SetFloat("MusicVolume", 0);
        else
            _audioMixer.audioMixer.SetFloat("MusicVolume", -80);
    }

    public void ChangeVolume(float volume)
    {
        volume = _sliderMusic.value;

        if (volume == 0)
            volume += 0.000001f;

        _audioMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    private void ContinueGame()
    {
        _pauseUI.DOAnchorPos(_hiddenPosition, _animationDuration);
        _pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
