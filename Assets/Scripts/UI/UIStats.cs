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
        _kill.text = "Убито: " + currentKill;
        _level.text = "Уровень: " + currentLevel;
        _timer.text = "Время игры: " + currentTime;
    }


}
