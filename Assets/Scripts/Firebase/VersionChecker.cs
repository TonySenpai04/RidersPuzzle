
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;

public class VersionChecker : MonoBehaviour
{
    public string currentVersion;
    public GameObject popup;
    public SliderController sliderController;
    public TextMeshProUGUI Txt;

    // Link tới file version.txt trên server (nhớ thay bằng link của bạn)
    public string versionCheckUrl = "https://drive.google.com/file/d/1w3FrRqeQqahO5H2lpWo_QCu7H4VR8K04/view?usp=sharing";

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
                Debug.Log("📥 Latest version từ server: " + latestVersion);

                Debug.Log("\n📥 Latest version from server: [" + latestVersion + "]");
                Debug.Log("\n📦 Current version from build: [" + currentVersion + "]");

                if (!string.IsNullOrEmpty(latestVersion) && latestVersion != currentVersion)
                {
                    popup.SetActive(true);
                    sliderController.HideSlider();
                    NotiManager.instance.ShowMultipleNotiRedDots(new List<string> { "new", "newbtn" });
                    Debug.Log("⚠️ Cần cập nhật phiên bản!");
                }
                else
                {
                    popup.SetActive(false);
                    sliderController.ShowSlider();
                    Debug.Log("✅ Phiên bản hiện tại đã là mới nhất!");
                }
            }
        }
    }
}

