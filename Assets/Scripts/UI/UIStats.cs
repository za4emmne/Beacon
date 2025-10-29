using UnityEngine;
using UnityEngine.UI;

public class UIStats : MonoBehaviour
{
    [SerializeField] private Text _kill;
    [SerializeField] private Text _timer;
    [SerializeField] private Text _coin;
    [SerializeField] private Text _level;

    public void CurrentStatsUpdate(int currentKill, int currentLevel, string currentTime)
    {
        _kill.text = "�����: " + currentKill;
        _level.text = "�������: " + currentLevel;
        _timer.text = "����� ����: " + currentTime;
    }


}
