using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class LevelDataInfo
{
    public Dictionary<Vector2Int, int> positions;  
    public bool isActive;
    public int move;
    public Vector2 startPos;
    public Vector2 endPos;
}
public class CSVReader : MonoBehaviour
{
    public TextAsset data;
    public Dictionary<int, LevelDataInfo> levelData;
     void Awake()
    {
        if (data == null)
        {
            Debug.LogError("Dữ liệu CSV chưa được gán vào trường 'data' trong Inspector.");
            return;
        }

        levelData = ReadLevelData(data);
        if (levelData == null || levelData.Count == 0)
        {
            Debug.LogWarning("Không có dữ liệu trong levelData.");
        }
        else
        {
            foreach (var level in levelData)
            {

                Debug.Log($"Level: {level.Key}"); // Hiển thị tên level
                Debug.Log(level.Value.positions.Count);

                if (level.Value.positions.Count == 0)
                {
                    Debug.LogWarning($"Level {level.Key} không có đối tượng.");
                }
                else
                {
                    
                    foreach (var entry in level.Value.positions)
                    {
                        // Debug vị trí và Object ID
                        Debug.Log($"Tọa độ: {entry.Key}, Object ID: {entry.Value}");
                    }
                }

                // Debug trạng thái isActive
                Debug.Log($"Level {level.Key} isActive: {level.Value.isActive}");
            }
        }
    }

    //Dictionary<int, Dictionary<Vector2Int, int>> ReadLevelData(TextAsset csvData)
    //{
    //    var levelData = new Dictionary<int, Dictionary<Vector2Int, int>>();

    //    if (csvData == null)
    //    {
    //        Debug.LogError("CSV Data không được gán trong Inspector.");
    //        return levelData;
    //    }

    //    // Đọc các dòng dữ liệu từ TextAsset
    //    var lines = csvData.text.Split('\n');

    //    // Đọc header (tọa độ cột)
    //    var headers = lines[0].Split(',');
    //    var coordinateColumns = new Dictionary<int, Vector2Int>();

    //    for (int col = 4; col < headers.Length - 4; col++) // Cột bắt đầu từ (5;0)
    //    {
    //        var coordText = headers[col].Trim().Replace("(", "").Replace(")", "").Split(';');
    //        // coordinateColumns[col] = new Vector2Int(1, col);
    //        var x = int.Parse(coordText[0]);
    //        var y = int.Parse(coordText[1]);
    //        coordinateColumns[col] = new Vector2Int(x, y);

    //    }

    //    for (int row = 1; row < lines.Length; row++) // Duyệt từng dòng dữ liệu
    //    {
    //        var values = lines[row].Split(',');

    //        // Kiểm tra nếu dòng dữ liệu trống hoặc không hợp lệ
    //        if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

    //        int level;
    //        if (!int.TryParse(values[0], out level)) continue; // Nếu không thể chuyển thành int, bỏ qua dòng này

    //        if (!levelData.ContainsKey(level))
    //        {
    //            levelData[level] = new Dictionary<Vector2Int, int>();
    //        }

    //        foreach (var col in coordinateColumns.Keys)
    //        {
    //            var coord = coordinateColumns[col];

    //            int objectId;
    //            if (int.TryParse(values[col].Trim(), out objectId))
    //            {
    //                levelData[level][coord] = objectId; 

    //            }
    //            else
    //            {
    //                Debug.LogWarning($"Invalid Object ID at Level: {level}, Coordinates: ({coord.x}, {coord.y})");
    //            }
    //        }
    //    }

    //    return levelData;
    //}
    Dictionary<int, LevelDataInfo> ReadLevelData(TextAsset csvData)
    {
        var levelData = new Dictionary<int, LevelDataInfo>();

        if (csvData == null)
        {
            Debug.LogError("CSV Data không được gán trong Inspector.");
            return levelData;
        }

        // Đọc các dòng dữ liệu từ TextAsset
        var lines = csvData.text.Split('\n');

        // Đọc header (tọa độ cột)
        var headers = lines[0].Split(',');
        var coordinateColumns = new Dictionary<int, Vector2Int>();
        // Đọc cột isActive (ví dụ cột cuối cùng)

     
        // Đọc các cột tọa độ
        for (int col = 3; col < headers.Length - 4; col++) // Cột bắt đầu từ (5;0)
        {
            var coordText = headers[col].Trim().Replace("(", "").Replace(")", "").Split(';');
            var x = int.Parse(coordText[0]);
            var y = int.Parse(coordText[1]);
            coordinateColumns[col] = new Vector2Int(x, y);
        }

        for (int row = 1; row < lines.Length; row++) // Duyệt từng dòng dữ liệu
        {
            var values = lines[row].Split(',');

            // Kiểm tra nếu dòng dữ liệu trống hoặc không hợp lệ
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            int level;
            if (!int.TryParse(values[0], out level)) continue; // Nếu không thể chuyển thành int, bỏ qua dòng này

            if (!levelData.ContainsKey(level))
            {
                levelData[level] = new LevelDataInfo
                {
                    positions = new Dictionary<Vector2Int, int>(),
                    isActive = false // Mặc định là false
                };
            }

            var levelInfo = levelData[level];

            // Đọc các cột tọa độ và ID đối tượng
            foreach (var col in coordinateColumns.Keys)
            {
                var coord = coordinateColumns[col];

                int objectId;
                if (int.TryParse(values[col].Trim(), out objectId))
                {
                    levelInfo.positions[coord] = objectId;
                }
                else
                {
                    Debug.LogWarning($"Invalid Object ID at Level: {level}, Coordinates: ({coord.x}, {coord.y})");
                }
            }
           // bool isActive;
            var headers2 = lines[1].Split(',');
            int a = int.Parse(headers2[1]);
            if (a == 1)
            {
                levelInfo.isActive=true;
            }
            else
            {
                levelInfo.isActive = false;
            }
            levelInfo.move = int.Parse(headers2[2]);
            //if (bool.TryParse(headers2[1], out isActive))
            //{
            //    levelInfo.isActive = isActive;
            //}
            //else
            //{
            //    Debug.LogWarning($"Invalid isActive value at Level: {level}");
            //}

        }

        return levelData;
    }



}
