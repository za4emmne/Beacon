using UnityEngine;
using UnityEngine.UI;
using YG.LanguageLegacy;

public class UIStats : MonoBehaviour
{
    [SerializeField] private Text _kill;
    [SerializeField] private Text _timer;
    [SerializeField] private Text _coin;
    [SerializeField] private Text _level;

    public void CurrentStatsUpdate(int currentKill, int currentLevel, string currentTime, int currentCoin)
    {
        _kill.text = LocalizationManager.Instance.GetTranslation("kill_text")
                        .Replace("{killCount}", currentKill.ToString());
        _level.text = LocalizationManager.Instance.GetTranslation("level_text")
                        .Replace("{level}", currentLevel.ToString());
        _timer.text = LocalizationManager.Instance.GetTranslation("time_text")
                        .Replace("{time}", currentTime);
        _coin.text = LocalizationManager.Instance.GetTranslation("coin_text")
                        .Replace("{countCoins}", currentCoin.ToString());
    }
}
