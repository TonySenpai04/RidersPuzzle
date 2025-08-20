
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class VersionChecker : MonoBehaviour
{
    public string currentVersion;
    public GameObject popup;
    public SliderController sliderController;
    public TextMeshProUGUI Txt;

    // Link tới file version.txt trên server (nhớ thay bằng link của bạn)
    public string versionCheckUrl = "https://drive.google.com/file/d/1w3FrRqeQqahO5H2lpWo_QCu7H4VR8K04/view?usp=sharing";
    public string goldPath => Path.Combine(Application.persistentDataPath, "version.json");

    void Start()
    {
        currentVersion = Application.version;
        Debug.Log("📦 Current version from build: " + currentVersion);

        StartCoroutine(CheckVersionOnline());
    }

    IEnumerator CheckVersionOnline()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(versionCheckUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {

                Debug.LogError("❌ Lỗi khi kiểm tra phiên bản: " + www.error);
            }
            else
            {
                string latestVersion = www.downloadHandler.text.Trim();
                string savedVersion = LoadSavedVersion();

                // Nếu chưa có file -> dùng currentVersion làm bản cũ để so sánh
                if (string.IsNullOrEmpty(savedVersion))
                {
                    savedVersion = currentVersion;
                    SaveVersionToFile(savedVersion);
                    NotiManager.instance.ShowMultipleNotiRedDots(new List<string> { "new", "newbtn" });
                    Debug.Log("📂 Không tìm thấy version.json, dùng currentVersion làm mặc định: " + savedVersion);
                }

                if (latestVersion != currentVersion)
                {
                    // Có version mới hơn hiện tại → yêu cầu cập nhật
                    popup.SetActive(true);
                    sliderController.HideSlider();
                    NotiManager.instance.ShowMultipleNotiRedDots(new List<string> { "new", "newbtn" });
                    Debug.Log("⚠️ Cần cập nhật phiên bản!");
                    SaveVersionToFile(latestVersion);
                }
                else if (savedVersion != latestVersion)
                {
                    // App đang ở bản mới nhất rồi, nhưng version.json vẫn là bản cũ → cập nhật file và hiển thị red dot
                    NotiManager.instance.ShowMultipleNotiRedDots(new List<string> { "new", "newbtn" });
                    SaveVersionToFile(latestVersion);
                    Debug.Log("📌 Đã cập nhật version.json vì có version mới nhưng app đã đúng phiên bản.");
                }
                else
                {
                    // Mọi thứ đều ok
                    popup.SetActive(false);
                    sliderController.ShowSlider();
                    Debug.Log("✅ Phiên bản hiện tại đã là mới nhất!");
                }

                //if (string.IsNullOrEmpty(savedVersion) || savedVersion != latestVersion)
                //{
                //    //if (!string.IsNullOrEmpty(latestVersion) && latestVersion != currentVersion)
                //    //{
                //    popup.SetActive(true);
                //    sliderController.HideSlider();
                //    NotiManager.instance.ShowMultipleNotiRedDots(new List<string> { "new", "newbtn" });
                //    Debug.Log("⚠️ Cần cập nhật phiên bản!");
                //    SaveVersionToFile(latestVersion);
                //}
                //else
                //{
                //    popup.SetActive(false);
                //    sliderController.ShowSlider();
                //    Debug.Log("✅ Phiên bản hiện tại đã là mới nhất!");
                //}
            }
        }
    }
    void SaveVersionToFile(string serverVersion)
    {
        var data = new VersionData { version = serverVersion };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(goldPath, json);
        Debug.Log($"💾 Đã lưu version server [{serverVersion}] vào version.json.");
    }

    string LoadSavedVersion()
    {
        if (File.Exists(goldPath))
        {
            string json = File.ReadAllText(goldPath);
            VersionData data = JsonUtility.FromJson<VersionData>(json);
            return data?.version ?? "";
        }
        return "";
    }

}
public class VersionData {
    public string version;
}


