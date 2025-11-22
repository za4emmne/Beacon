using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [SerializeField] private Text timerText; // Ссылка на UI Text объект

    private float _startTime; // Время начала таймера
    private bool _isRunning = false; // Флаг работы таймера

    private void Awake()
    {
        Instance = this;
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
        if (_isRunning)
        {
            float currentTime = Time.time - _startTime;

            int minutes = (int)(currentTime / 60);
            int seconds = (int)(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Метод для получения текущего времени в секундах
    public float GetCurrentTime()
    {
        return Time.time - _startTime;
    }

    public string GetCurrentTimeText()
    {
        float currentTime = Time.time - _startTime;

        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);

        return string.Format("{0:00 мин} {1:00 сек}", minutes, seconds);
    }

    public float GetFloatTime()
    {
        return Time.time - _startTime;
    }
}