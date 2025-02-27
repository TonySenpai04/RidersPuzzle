using Firebase.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushNotificationManager : MonoBehaviour
{
    void Start()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("FCM Token: " + token.Token);
    }

    void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Thông báo nhận được: " + e.Message.Notification.Body);
    }
}
