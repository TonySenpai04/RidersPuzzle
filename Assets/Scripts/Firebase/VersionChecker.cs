using System;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;

public class VersionChecker : MonoBehaviour
{
    public string currentVersion = "1.0.0";

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // ✅ Cấu hình đúng với Firebase 12.6.0
                var config = FirebaseRemoteConfig.DefaultInstance.ConfigSettings;
                config.MinimumFetchIntervalInMilliseconds = 0;
                var configSettings = FirebaseRemoteConfig.DefaultInstance.ConfigSettings;
                configSettings.MinimumFetchIntervalInMilliseconds = 0;


                FetchRemoteConfig();
            }
            else
            {
                Debug.LogError("Firebase dependencies not available: " + task.Result);
            }
        });
    }

    void FetchRemoteConfig()
    {
        FirebaseRemoteConfig.DefaultInstance.FetchAsync().ContinueWithOnMainThread(fetchTask =>
        {
            if (fetchTask.IsCompletedSuccessfully)
            {
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(_ =>
                {
                    string latestVersion = FirebaseRemoteConfig.DefaultInstance.GetValue("latest_version").StringValue;
                    Debug.Log("Latest version from Remote Config: " + latestVersion);

                    if (!string.IsNullOrEmpty(latestVersion) && latestVersion != currentVersion)
                    {
                        Debug.Log("⚠️ Cần cập nhật phiên bản!");
                        // TODO: Gọi popup bắt cập nhật
                    }
                    else
                    {
                        Debug.Log("⚠️ Không cần cập nhật phiên bản!");
                    }
                });
            }
            else
            {
                Debug.LogError("⚠️ Fetch Remote Config thất bại.");
            }
        });
    }
}
