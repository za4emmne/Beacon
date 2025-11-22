using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YG;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    private Dictionary<string, string> _translations;

    public string currentLang => YG2.lang;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке новой сцены
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Если вдруг появился дубликат — удаляем его сразу
        }
        LoadLocalization(currentLang);
    }

    private void LoadLocalization(string lang)
    {
        TextAsset file = Resources.Load<TextAsset>($"Localization/{lang}"); // Пример: "Localization/ru"
        if (file != null)
        {
            _translations = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(file.text);
        }
        else
        {
            Debug.LogError($"Localization file not found: Resources/Localization/{lang}.json");
            _translations = new Dictionary<string, string>();
        }
    }

    public string GetTranslation(string key)
    {
        if (_translations != null && _translations.ContainsKey(key))
            return _translations[key];
        return key;
    }
}
