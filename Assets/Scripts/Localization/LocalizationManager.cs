using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    private Dictionary<string, string> localizedText;
    private string currentLanguage = "vi"; // Mặc định là tiếng Việt

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadLocalizationData();
    }

    private void LoadLocalizationData()
    {
        localizedText = new Dictionary<string, string>();

        // Đường dẫn file CSV trong Resources
        TextAsset csvFile = Resources.Load<TextAsset>($"Localization/Localize");
        if (csvFile == null)
        {
            Debug.LogError("Localization file not found!");
            return;
        }

        // Đọc từng dòng trong CSV
        using (StringReader reader = new StringReader(csvFile.text))
        {
            string line;
            bool isHeader = true;

            while ((line = reader.ReadLine()) != null)
            {
                // Bỏ qua header
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                string[] entries = line.Split(';');
                if (entries.Length >= 5) // Đảm bảo đúng cấu trúc
                {
                    string key = entries[1]; // Key
                    string languageText = currentLanguage == "vi" ? entries[4] : entries[3]; // Chọn ngôn ngữ
                    localizedText[key] = languageText;
                }
            }
        }
    }

    public string GetLocalizedValue(string key)
    {
        if (localizedText.TryGetValue(key, out string value))
        {
            return value;
        }

        Debug.LogWarning($"Key {key} not found in localization data.");
        return key;
    }

    public void SetLanguage(string language)
    {
        currentLanguage = language;
        LoadLocalizationData();
    }
}
