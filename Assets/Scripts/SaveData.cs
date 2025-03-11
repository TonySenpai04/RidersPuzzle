using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
[Serializable]
public class LevelProgressData
{
    public int levelIndex;
    public bool isUnlocked;
    public bool isComplete;
}

[Serializable]
public class UnlockHeroData
{
    public List<int> seenHeroIds;

    public UnlockHeroData(List<int> ids)
    {
        seenHeroIds = ids;
    }
}

[Serializable]
public class GoldData
{
    public int amount;
}

public class LanguageData
{
    public int languageCode;
}
[Serializable]
public class SoundData
{
    public bool isMute;
}

[Serializable]
public class SeenObjectsData
{
    public List<int> seenObjectIds;

    public SeenObjectsData(List<int> ids)
    {
        seenObjectIds = ids;
    }
}
[Serializable]
public class SelectedHeroData
{
    public int SelectedHeroID;
}

[Serializable]
public class FirstPlayData
{
    public bool isFirst;
}

[System.Serializable]
public class DailyGiftData
{
    public string lastClaimDate = "";
}
public class SaveData : MonoBehaviour
{
    
}
