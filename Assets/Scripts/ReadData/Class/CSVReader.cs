using System;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : IReadData
{
 
    public CSVReader()
    {
        
    }
   public  Dictionary<int, LevelDataInfo> ReadLevelData(TextAsset csvData)
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
  
                }
            }
            levelInfo.level=level;
            int a = int.Parse(values[1]);
            if (a == 1)
            {
                levelInfo.isActive=true;
            }
            else
            {
                levelInfo.isActive = false;
            }
            levelInfo.move = int.Parse(values[2]);
            var startPos = values[headers.Length - 3].Trim().Split(';');
            levelInfo.startPos = new Vector2(float.Parse(startPos[0]), float.Parse(startPos[1]));
            var endPos = values[headers.Length - 2].Trim().Split(';');
            levelInfo.endPos = new Vector2(float.Parse(endPos[0]), float.Parse(endPos[1]));
            var difficulty = values[headers.Length - 1].Trim();
            levelInfo.difficulty = difficulty;

        }

        return levelData;
    }



}
