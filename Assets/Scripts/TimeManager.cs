using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    private string timeAPI = "https://timeapi.io/api/Time/current/zone?timeZone=Asia/Ho_Chi_Minh";
    public static TimeManager Instance;
    public string ServerDate { get; private set; }
    public bool IsTimeFetched { get; private set; } = false; // Đánh dấu đã lấy xong thời gian

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(GetServerTime()); // Lấy thời gian ngay khi game khởi động
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator GetServerTime()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(timeAPI))
        {
            request.certificateHandler = new BypassCertificate();
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = request.downloadHandler.text;
                ServerTimeData timeData = JsonUtility.FromJson<ServerTimeData>(jsonResult);
                ServerDate = DateTime.Parse(timeData.dateTime).ToString("yyyy-MM-dd");
                IsTimeFetched = true;
                Debug.Log("Thời gian server: " + ServerDate);
            }
            else
            {
                Debug.Log("Lỗi lấy thời gian từ server!");
            }
        }
    }

    [Serializable]
    private class ServerTimeData
    {
        public string dateTime;
    }

    private class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
}
