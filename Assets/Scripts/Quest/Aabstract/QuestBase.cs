using Firebase.Database;
using Firebase.Extensions;
using System;
using System.IO;
using UnityEngine;

public abstract class QuestBase
{
    public string questId;
    [NonSerialized] public string description;
    public int reward;
    public bool isReward;
    public string groupId;

    public virtual void UpdateProgress(int progress)
    {

    }
    public virtual void UpdateProgress(int progress1, int progress2)
    {

    }
    public abstract bool CheckCompletion();
    public virtual void SaveQuest()
    {
        string path = Application.persistentDataPath + $"/quest_{questId}.json";
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(path, json);
        SaveQuestToFirebase();

    }
    public virtual void LoadQuest()
    {
        string path = Application.persistentDataPath + $"/quest_{questId}.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            JsonUtility.FromJsonOverwrite(json, this);
            LoadQuestFromFirebase();

        }
        else
        {
            LoadQuestFromFirebase();
        }

    }
    public virtual void DeleteQuest()
    {
        string path = Application.persistentDataPath + $"/quest_{questId}.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Quest {questId} deleted.");
        }
        if (FirebaseDataManager.Instance.GetCurrentUser() != null)
        {
            string userId = FirebaseDataManager.Instance.GetCurrentUser().UserId;
            string questKey = $"quest_{questId}";

            FirebaseDatabase.DefaultInstance.RootReference
                .Child("users")
                .Child(userId)
                .Child("quests")
                .Child(questKey)
                .RemoveValueAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                        Debug.Log($"🗑️ Đã xoá quest {questId} khỏi Firebase.");
                    else
                        Debug.LogError($"❌ Lỗi khi xoá quest {questId} khỏi Firebase: {task.Exception}");
                });
        }
        else
        {
            Debug.Log("⚠️ Chưa đăng nhập – không thể xoá quest khỏi Firebase.");
        }
    }
    public virtual Tuple<int,int> GetProgress()
    {
        return Tuple.Create(0,0);
    }
    public void OnBeforeSerialize() { }
    public virtual void SaveQuestToFirebase()
    {
        if (FirebaseDataManager.Instance.GetCurrentUser() == null)
        {
            Debug.Log("❌ Chưa đăng nhập - Không thể lưu quest lên Firebase.");
            return;
        }

        string json = JsonUtility.ToJson(this);
        string questKey = $"quest_{questId}";

        FirebaseDatabase.DefaultInstance.RootReference
            .Child("users")
            .Child(FirebaseDataManager.Instance.GetCurrentUser().UserId)
            .Child("quests")
            .Child(questKey)
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log($"✅ Đã lưu quest {questId} lên Firebase.");
                else
                    Debug.LogError($"❌ Lỗi khi lưu quest {questId}: {task.Exception}");
            });
    }
    public virtual void LoadQuestFromFirebase()
    {
        if (FirebaseDataManager.Instance.GetCurrentUser() == null)
        {
            Debug.Log("❌ Chưa đăng nhập - Không thể load quest từ Firebase.");
            return;
        }

        string questKey = $"quest_{questId}";

        FirebaseDatabase.DefaultInstance.RootReference
            .Child("users")
            .Child(FirebaseDataManager.Instance.GetCurrentUser().UserId)
            .Child("quests")
            .Child(questKey)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    string json = task.Result.GetRawJsonValue();
                    JsonUtility.FromJsonOverwrite(json, this);
                    Debug.Log($"✅ Đã load quest {questId} từ Firebase vào instance hiện tại.");
                }
                else
                {
                    Debug.Log($"📂 Không có dữ liệu quest {questId} trên Firebase.");
                }
            });
    }
    public virtual void SyncLocalQuestsToFirebaseIfNotExist()
    {
        if (FirebaseDataManager.Instance.GetCurrentUser() == null)
        {
            Debug.Log("❌ Chưa đăng nhập - Không thể đồng bộ quest.");
            return;
        }

        string path = Application.persistentDataPath + $"/quest_{questId}.json";
        string userId = FirebaseDataManager.Instance.GetCurrentUser().UserId;

        if (!File.Exists(path))
        {
            Debug.LogWarning($"⚠️ Không tìm thấy file local của quest {questId} để đồng bộ.");
            return;
        }

        string json = File.ReadAllText(path);
        string questKey = $"quest_{questId}";

        FirebaseDatabase.DefaultInstance.RootReference
            .Child("users")
            .Child(userId)
            .Child("quests")
            .Child(questKey)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (!task.Result.Exists)
                    {
                        FirebaseDatabase.DefaultInstance.RootReference
                            .Child("users")
                            .Child(userId)
                            .Child("quests")
                            .Child(questKey)
                            .SetRawJsonValueAsync(json)
                            .ContinueWithOnMainThread(uploadTask =>
                            {
                                if (uploadTask.IsCompleted)
                                    Debug.Log($"☁️ Đã đồng bộ quest {questId} từ local lên Firebase.");
                                else
                                    Debug.LogError($"❌ Lỗi khi đồng bộ quest {questId}: {uploadTask.Exception}");
                            });
                    }
                    else
                    {
                        Debug.Log($"✅ Quest {questId} đã tồn tại trên Firebase.");
                    }
                }
            });
    }



}
