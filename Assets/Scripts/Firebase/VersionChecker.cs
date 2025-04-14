//using System;
//using UnityEngine;
//using Firebase;
//using Firebase.Extensions;
//using Firebase.RemoteConfig;
//using TMPro;

//public class VersionChecker : MonoBehaviour
//{
//    public string currentVersion;
//    public GameObject popup;
//    public SliderController sliderController;
//    public TextMeshProUGUI Txt;
//    void Start()
//    {
//        currentVersion = Application.version;
//        Txt.text = "📦 Current version from build: " + currentVersion;
//        Debug.Log("📦 Current version from build: " + currentVersion);

//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//        {
//            if (task.Result == DependencyStatus.Available)
//            {
//                // ✅ Cài đặt fetch không cache
//                var configSettings = FirebaseRemoteConfig.DefaultInstance.ConfigSettings;
//                configSettings.MinimumFetchIntervalInMilliseconds = 0;
//                configSettings.FetchTimeoutInMilliseconds = 1000;

//                FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(configSettings).ContinueWithOnMainThread(_ =>
//                {
//                    // ✅ Đặt giá trị mặc định (nếu có)
//                    FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(new System.Collections.Generic.Dictionary<string, object>
//                {
//                    { "latest_version", "1.2.0" } // fallback nếu không fetch được
//                }).ContinueWithOnMainThread(__ =>
//                {
//                    FetchRemoteConfig();
//                });
//                });
//            }
//            else
//            {
//                Txt.text = "Firebase dependencies not available: " + task.Result;
//                Debug.LogError("Firebase dependencies not available: " + task.Result);
//            }
//        });
//    }
//    void FetchRemoteConfig()
//    {
//        Txt.text = "👉 Bắt đầu fetch remote config...";
//        Debug.Log("👉 Bắt đầu fetch remote config...");
//        FirebaseRemoteConfig.DefaultInstance.FetchAsync().ContinueWithOnMainThread(fetchTask =>
//        {
//            if (fetchTask.IsCompletedSuccessfully)
//            {
//                Txt.text = "✅ Fetch thành công!";
//                Debug.Log("✅ Fetch thành công!");

//                FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(activateTask =>
//                {
//                    Debug.Log("✅ Activate config: " + activateTask.IsCompletedSuccessfully);

//                    string latestVersion = FirebaseRemoteConfig.DefaultInstance.GetValue("latest_version").StringValue;
//                    string a = "📥 Latest version from Remote Config: [" + latestVersion + "]";
//                    string b = "📦 Current version from build: [" + currentVersion + "]";
//                    Txt.text = a + b;
//                    Debug.Log("📥 Latest version from Remote Config: [" + latestVersion + "]");
//                    Debug.Log("📦 Current version from build: [" + currentVersion + "]");

//                    if (!string.IsNullOrEmpty(latestVersion) && latestVersion.Trim() != currentVersion.Trim())
//                    {
//                        popup.SetActive(true);
//                        sliderController.HideSlider();
//                        Debug.Log("⚠️ Cần cập nhật phiên bản!");
//                    }
//                    else
//                    {
//                        popup.SetActive(false);
//                        sliderController.ShowSlider();
//                        Debug.Log("✅ Phiên bản hiện tại đã là mới nhất!");
//                    }
//                });
//            }
//            else
//            {
//                Debug.LogError("❌ Fetch Remote Config thất bại: " + fetchTask.Exception?.Message);
//            }
//        });
//    }

//}
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

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

