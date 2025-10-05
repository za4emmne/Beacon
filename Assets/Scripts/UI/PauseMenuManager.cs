using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private AudioMixerGroup _audioMixer;
    [SerializeField] private UnityEngine.UI.Button _continueButton;
    [SerializeField] private UnityEngine.UI.Slider _sliderMusic;
    [SerializeField] private UnityEngine.UI.Toggle _toggleMusic;


    private void Start()
    {
        _pauseScreen.SetActive(false);
    }

    public void Pause()
    {
        _continueButton.onClick.AddListener(ContinueGame);
        _pauseScreen.SetActive(true);
        Time.timeScale = 0f;
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
        _pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
