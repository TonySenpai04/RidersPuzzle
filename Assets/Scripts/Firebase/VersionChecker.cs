using System;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;

public class VersionChecker : MonoBehaviour
{
    public string currentVersion ;
    public GameObject popup;
    public SliderController sliderController;
    void Start()
    {
        currentVersion = Application.version;
        Debug.Log("📦 Current version from build: " + currentVersion);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // ✅ Cài đặt fetch không cache
                var configSettings = FirebaseRemoteConfig.DefaultInstance.ConfigSettings;
                configSettings.MinimumFetchIntervalInMilliseconds = 0;
                configSettings.FetchTimeoutInMilliseconds = 1000;

                FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(configSettings).ContinueWithOnMainThread(_ =>
                {
                    // ✅ Đặt giá trị mặc định (nếu có)
                    FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(new System.Collections.Generic.Dictionary<string, object>
                {
                    { "latest_version", "1.2.0" } // fallback nếu không fetch được
                }).ContinueWithOnMainThread(__ =>
                {
                    FetchRemoteConfig();
                });
                });
            }
            else
            {
                Debug.LogError("Firebase dependencies not available: " + task.Result);
            }
        });
    }

    //void Start()
    //{
    //    currentVersion = Application.version;
    //    Debug.Log("📦 Current version from build: " + currentVersion);
    //    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.Result == DependencyStatus.Available)
    //        {
    //            // ✅ Cấu hình đúng với Firebase 12.6.0
    //            var config = FirebaseRemoteConfig.DefaultInstance.ConfigSettings;
    //            config.MinimumFetchIntervalInMilliseconds = 0;
    //            var configSettings = FirebaseRemoteConfig.DefaultInstance.ConfigSettings;
    //            configSettings.MinimumFetchIntervalInMilliseconds = 0;
    //            configSettings.FetchTimeoutInMilliseconds = 1000;


    //            FetchRemoteConfig();
    //        }
    //        else
    //        {
    //            Debug.LogError("Firebase dependencies not available: " + task.Result);
    //        }
    //    });
    //}

    void FetchRemoteConfig()
    {
        Debug.Log("👉 Bắt đầu fetch remote config...");
        FirebaseRemoteConfig.DefaultInstance.FetchAsync().ContinueWithOnMainThread(fetchTask =>
        {
            if (fetchTask.IsCompletedSuccessfully)
            {
                Debug.Log("✅ Fetch thành công!");

                FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(activateTask =>
                {
                    Debug.Log("✅ Activate config: " + activateTask.IsCompletedSuccessfully);

                    string latestVersion = FirebaseRemoteConfig.DefaultInstance.GetValue("latest_version").StringValue;
                    Debug.Log("📥 Latest version from Remote Config: [" + latestVersion + "]");
                    Debug.Log("📦 Current version from build: [" + currentVersion + "]");

                    if (!string.IsNullOrEmpty(latestVersion) && latestVersion.Trim() != currentVersion.Trim())
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
                });
            }
            else
            {
                Debug.LogError("❌ Fetch Remote Config thất bại: " + fetchTask.Exception?.Message);
            }
        });
    }

}
