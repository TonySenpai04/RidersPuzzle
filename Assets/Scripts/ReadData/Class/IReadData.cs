using System;
using System.Collections.Generic;
using UnityEngine;

public interface IReadData
{
   public  Dictionary<int, LevelDataInfo> ReadLevelData(TextAsset csvData);
}
[Serializable]
public class LevelDataInfo
{
    public Dictionary<Vector2Int, int> positions;
    public bool isActive;
    public int move;
    public Vector2 startPos;
    public Vector2 endPos;
}