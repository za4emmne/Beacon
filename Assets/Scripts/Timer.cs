using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [SerializeField] private Text timerText;

    private float _startTime;
    private bool _isRunning = false;
    private int _lastMinutes = -1;
    private int _lastSeconds = -1;

    private void Awake()
    {
        Instance = this;
    }
    
    private void OnDestroy()
    {
        Instance = null;
    }
    
    private void Start()
    { 
        StartTimer();
    }

    public void StartTimer()
    {
        _startTime = Time.time;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    private void Update()
    {
        if (!_isRunning) return;

        float currentTime = Time.time - _startTime;
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);

        // Обновляем только при изменении значений
        if (minutes != _lastMinutes || seconds != _lastSeconds)
        {
            _lastMinutes = minutes;
            _lastSeconds = seconds;
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public float GetCurrentTime()
    {
        return Time.time - _startTime;
    }

    public string GetCurrentTimeText()
    {
        float currentTime = Time.time - _startTime;
        string timeMin = LocalizationManager.Instance.GetTranslation("min_text");
        string timeSec = LocalizationManager.Instance.GetTranslation("sec_text");
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        return $"{minutes:00} {timeMin} {seconds:00} {timeSec}";
    }

    public float GetFloatTime()
    {
        return Time.time - _startTime;
    }
}