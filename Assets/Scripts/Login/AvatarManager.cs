using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager instance;
    public List<Sprite> avatarList; 
    public Image avatarDisplay; 
    public int selectedAvatarIndex = 0;
    public AccountAvatar avatar;
    [SerializeField] private Transform content;
    [SerializeField] private List<AccountAvatar> avatars;
    public string avtPath => Path.Combine(Application.persistentDataPath, "AvtData.json");
    private void Awake()
    {
        instance = this;
        for(int i=0;i<avatarList.Count;i++)
        {
            AccountAvatar avt = Instantiate(avatar, content);
            avt.SetData(i, avatarList[i]);
            avt.button.onClick.AddListener(() => SelectAvatar(avt.index));
            avt.button.onClick.AddListener(() => FirebaseDataManager.Instance.SaveData(
                LevelManager.instance.GetAllLevelComplete(), GoldManager.instance.GetGold(),
                      SaveGameManager.instance.LoadAllProgress(), HeroManager.instance.GetUnlockHeroID()));
            avatars.Add(avt);

        }
        LoadLocal();
        SelectAvatar(selectedAvatarIndex);
    }
    public void SaveAvt()
    {
        var data = new AvatarData { avatarIndex = selectedAvatarIndex };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(avtPath, json);


    }
    public void LoadLocal()
    {
        if (File.Exists(avtPath))
        {
            string json = File.ReadAllText(avtPath);
            var gold = JsonUtility.FromJson<AvatarData>(json);
            this.selectedAvatarIndex = gold.avatarIndex;
        }
    }
    public void SelectAvatar(int index)
    {
        if (index >= 0 && index < avatarList.Count)
        {
            foreach(var avt in avatars)
            {
                avt.UnSelect();
            }
            selectedAvatarIndex = index;
            avatars[index].Select();
            avatarDisplay.sprite = avatarList[index];
            SaveAvt();
        }
    }

}
[Serializable]
public class AvatarData
{
    public int avatarIndex;
}
