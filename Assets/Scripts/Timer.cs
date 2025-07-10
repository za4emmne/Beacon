using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text timerText; // ������ �� UI Text ������

    private float _startTime; // ����� ������ �������
    private bool _isRunning = false; // ���� ������ �������

    private void Start()
    {
        // ������ ������ ������������� ��� ������
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

            // �������������� ������ � �������� ������ (00:00)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // ����� ��� ��������� �������� ������� � ��������
    public float GetCurrentTime()
    {
        return Time.time - _startTime;
    }
}