using Firebase.Messaging;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PushNotificationManager : MonoBehaviour
{
    public TextMeshProUGUI txt;
    void Start()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;

        // Ép Firebase lấy token (cách này hữu ích để debug)
        Firebase.Messaging.FirebaseMessaging.GetTokenAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                string token = task.Result;
                Debug.Log("🎯 Token thủ công: " + token);
                txt.text = token;
            }
            else
            {
                txt.text = "Lỗi khi lấy token:" + task.Exception;
                Debug.LogError("🚫 Lỗi khi lấy token: " + task.Exception);
            }
        });
    }


    void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        txt.text = token.Token;
        Debug.Log("FCM Token: " + token.Token);
    }

    void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        if (e.Message.Data.ContainsKey("url"))
        {
            string url = e.Message.Data["url"];
            Debug.Log("Mở đường link: " + url);
            Application.OpenURL(url); // Mở trình duyệt với URL nhận được
        }
        else
        {
            Debug.Log("Thông báo nhận được: " + e.Message.Notification.Body);
        }
    }
}
