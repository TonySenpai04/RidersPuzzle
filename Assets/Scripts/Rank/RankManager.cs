using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;

public class RankManager : MonoBehaviour
{
    public static RankManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        FetchRank();
    }
    public void FetchRank()
    {
        GetRankByLevel(rankList =>
        {
            Debug.Log("--- Rank By Level ---");
            if (rankList != null)
            {
                foreach (var player in rankList)
                {
                    Debug.Log($"Name: {player.name}, Level: {player.totalLevel}");
                }
            }
        });

        GetRankByGold(rankList =>
        {
            Debug.Log("--- Rank By Gold ---");
            if (rankList != null)
            {
                foreach (var player in rankList)
                {
                    Debug.Log($"Name: {player.name}, Gold: {player.gold}");
                }
            }
        });
    }

    public void GetRankByLevel(System.Action<List<PlayerData>> onResult)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users")
            .OrderByChild("playerData/totalLevel")
            .LimitToLast(100) // Get top 100 (Firebase sorts ascending, so we get the highest values at the end)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("❌ GetRankByLevel failed: " + task.Exception);
                    onResult?.Invoke(null);
                    return;
                }

                if (task.Result.Exists)
                {
                    List<PlayerData> rankList = new List<PlayerData>();
                    foreach (var userSnapshot in task.Result.Children)
                    {
                        string json = userSnapshot.Child("playerData").GetRawJsonValue();
                        if (!string.IsNullOrEmpty(json))
                        {
                            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                            if (data != null)
                            {
                                rankList.Add(data);
                            }
                        }
                    }

                    // Firebase returns ascending order, so reverse to get descending (highest level first)
                    rankList.Reverse();
                    onResult?.Invoke(rankList);
                }
                else
                {
                    onResult?.Invoke(new List<PlayerData>());
                }
            });
    }

    public void GetRankByGold(System.Action<List<PlayerData>> onResult)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users")
            .OrderByChild("playerData/gold")
            .LimitToLast(100)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("❌ GetRankByGold failed: " + task.Exception);
                    onResult?.Invoke(null);
                    return;
                }

                if (task.Result.Exists)
                {
                    List<PlayerData> rankList = new List<PlayerData>();
                    foreach (var userSnapshot in task.Result.Children)
                    {
                        string json = userSnapshot.Child("playerData").GetRawJsonValue();
                        if (!string.IsNullOrEmpty(json))
                        {
                            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                            if (data != null)
                            {
                                rankList.Add(data);
                            }
                        }
                    }

                    // Reverse to get descending order
                    rankList.Reverse();
                    onResult?.Invoke(rankList);
                }
                else
                {
                    onResult?.Invoke(new List<PlayerData>());
                }
            });
    
    }
}
