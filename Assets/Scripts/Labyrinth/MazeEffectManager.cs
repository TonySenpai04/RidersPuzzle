using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MazeEffectManager : MonoBehaviour
{
    private string localFilePath => Path.Combine(Application.persistentDataPath, "mazeEffect.json");
    [SerializeField] private List<MazeEffect> mazeEffects;
    public MazeEffect CurrentEffect { get; private set; }
    public static MazeEffectManager instance;

    private void Awake()
    {
        instance = this;
        mazeEffects = new List<MazeEffect>()
        {
            new FriendlyMaze(),
            new WealthyMaze(),
            new GreedyMaze(),
            new AngryMaze(),
            new PeacefulMaze()
        };

    }

    private void Start()
    {
        StartCoroutine(WaitForServerTime());
    }

    private IEnumerator WaitForServerTime()
    {
        while (!TimeManager.Instance.IsTimeFetched)
        {
            yield return null;
        }

        LoadDailyMazeEffect();
    }

    public void LoadDailyMazeEffect()
    {
        string today = TimeManager.Instance.ServerDate;
        FirebaseUser currentUser = FirebaseDataManager.Instance.GetCurrentUser();

        if (currentUser == null)
        {
            LoadFromJsonLocal(today);
        }
        else
        {
            LoadFromFirebase(today, currentUser.UserId);
        }
    }

    private void LoadFromJsonLocal(string today)
    {
        if (File.Exists(localFilePath))
        {
            string json = File.ReadAllText(localFilePath);
            MazeEffectData data = JsonUtility.FromJson<MazeEffectData>(json);

            if (data.lastMazeEffectDate == today)
            {
                CurrentEffect = GetAllEffects()[data.lastMazeEffectIndex];
            }
            else
            {
                SetNewLocalEffect(today);
            }
        }
        else
        {
            SetNewLocalEffect(today);
        }

        Debug.Log($"[LOCAL JSON] Hiệu ứng mê cung hôm nay: {CurrentEffect.Name} - {CurrentEffect.Description}");
    }

    private void SetNewLocalEffect(string today)
    {
        var effects = GetAllEffects();
        int index = UnityEngine.Random.Range(0, effects.Count);
        CurrentEffect = effects[index];

        MazeEffectData data = new MazeEffectData
        {
            lastMazeEffectDate = today,
            lastMazeEffectIndex = index
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(localFilePath, json);
    }

    private async void LoadFromFirebase(string today, string userId)
    {
        DataSnapshot snapshot = await FirebaseDatabase.DefaultInstance.RootReference
            .Child("users")
            .Child(userId)
            .Child("mazeEffectData")
            .GetValueAsync();

        if (snapshot.Exists)
        {
            string json = snapshot.GetRawJsonValue();
            MazeEffectData data = JsonUtility.FromJson<MazeEffectData>(json);

            if (data.lastMazeEffectDate == today)
            {
                CurrentEffect = GetAllEffects()[data.lastMazeEffectIndex];
            }
            else
            {
                SetNewFirebaseEffect(today, userId);
            }
        }
        else
        {
            SetNewFirebaseEffect(today, userId);
        }

        Debug.Log($"[FIREBASE] Hiệu ứng mê cung hôm nay: {CurrentEffect.Name} - {CurrentEffect.Description}");
    }

    private async void SetNewFirebaseEffect(string today, string userId)
    {
        var effects = GetAllEffects();
        int index = UnityEngine.Random.Range(0, effects.Count);
        CurrentEffect = effects[index];

        MazeEffectData data = new MazeEffectData
        {
            lastMazeEffectDate = today,
            lastMazeEffectIndex = index
        };

        string json = JsonUtility.ToJson(data);
        await FirebaseDatabase.DefaultInstance
            .RootReference
            .Child("users")
            .Child(userId)
            .Child("mazeEffectData")
            .SetRawJsonValueAsync(json);
    }

    private List<MazeEffect> GetAllEffects()
    {
        return mazeEffects;
        
    }

    public void ApplyDailyEffect()
    {
        CurrentEffect?.ApplyEffect();
    }
}


