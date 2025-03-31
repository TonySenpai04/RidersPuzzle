using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using TMPro;
using UnityEngine;

public class ReadCSVLocalizeStory : ReadCSVLocalizeBase
{
   

    public override void LoadLocalization(int currentLanguage, Dictionary<string, string> localizedTexts,
        Dictionary<string, TMP_FontAsset> localizedFonts, TextAsset textAsset, Dictionary<string, string> richText)
    {
        string[] rawLines = textAsset.text.Split('\n');
        List<string> fullLines = new List<string>();

        // Gộp các dòng nếu có dòng bị ngắt bên trong dấu ngoặc kép
        StringBuilder currentLine = new StringBuilder();
        int quoteCount = 0;

        foreach (string rawLine in rawLines)
        {
            currentLine.Append(rawLine.TrimEnd('\r')); // Loại bỏ \r nếu có
            quoteCount += CountQuotes(rawLine);

            // Nếu đủ dấu ngoặc kép (đóng mở đều), thì đây là 1 dòng hoàn chỉnh
            if (quoteCount % 2 == 0)
            {
                fullLines.Add(currentLine.ToString());
                currentLine.Clear();
                quoteCount = 0;
            }
            else
            {
                currentLine.Append("\n"); // Nếu chưa đủ, thì dòng tiếp theo vẫn thuộc cùng 1 dòng dữ liệu
            }
        }

        for (int i = 1; i < fullLines.Count; i++)
        {
            string[] values = csvSplit.Split(fullLines[i]);

            for (int j = 0; j < values.Length; j++)
            {
                values[j] = values[j].Trim().Trim('"').Replace("\"\"", "\""); // Replace "" → "
            }

            if (values.Length >= 7)
            {
                string key = values[1];
                string enText = values[3];
                string viText = values[5];
                string fontName = currentLanguage == 5 ? values[6] : values[4];
                var richTxt = values[5];

                localizedTexts[key] = currentLanguage == 5 ? viText : enText;
                localizedFonts[key] = Resources.Load<TMP_FontAsset>($"Fonts/{fontName}");
                richText[key] = richTxt;
            }
        }
    }

    // Đếm số dấu ngoặc kép trong dòng
    private int CountQuotes(string line)
    {
        int count = 0;
        foreach (char c in line)
        {
            if (c == '"') count++;
        }
        return count;
    }

}