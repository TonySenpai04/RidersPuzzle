using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingEvil : HiddenObject
{
    public GameObject jyamatoPrefab;
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }

        PlaySFX();
        var currentPos = PlayerController.instance.movementController.GetPos();
        int mapRows = LevelManager.instance.GetGrid().rows;
        int mapCols = LevelManager.instance.GetGrid().cols;
        List<Vector2Int> spawnPositions = new List<Vector2Int>();

        for (int i = 0; i < LevelManager.instance.GetGrid().rows; i++)
        {
            if (i != currentPos.Item1)
            {
                GameObject obj = LevelManager.instance.CheckForHiddenObject(i, currentPos.Item2);
                if (obj == null)
                    spawnPositions.Add(new Vector2Int(i, currentPos.Item2));
            }
        }
        for (int j = 0; j < LevelManager.instance.GetGrid().cols; j++)
        {
            if (j != currentPos.Item2)
            {
                GameObject obj = LevelManager.instance.CheckForHiddenObject(currentPos.Item1, j);
                if (obj == null)
                    spawnPositions.Add(new Vector2Int(currentPos.Item1, j));
            }
        }
        if (spawnPositions.Count > 0)
        {

            foreach (Vector2Int spawnPos in spawnPositions)
            {
                if (LevelManager.instance.CheckForHiddenObject(spawnPos.x, spawnPos.y) == null)
                {
                    if (spawnPos.x >= 0 && spawnPos.x < mapRows && spawnPos.y >= 0
                        && spawnPos.y < mapCols && spawnPos != LevelManager.instance.GetCurrentLevelData().endPos)
                    {
                        GameObject cell = LevelManager.instance.GetGrid().grid[(int)spawnPos.x, (int)spawnPos.y];

                        GameObject hiddenObject = Instantiate(jyamatoPrefab, cell.transform.position, Quaternion.identity);

                        hiddenObject.SetActive(true);
                        float screenWidth = Camera.main.orthographicSize * 2 * 9f / 16f;
                        float cellSize = (float)(screenWidth - 0.1 * (6 - 1)) / 6 - 0.1f;
                        hiddenObject.transform.localScale = new Vector3(cellSize, cellSize, 1);
                        LevelManager.instance.AddHiddenObjectToCurrentLevel(spawnPos.x, spawnPos.y, hiddenObject);
                        hiddenObject.transform.SetParent(cell.transform);
                    }
                }


            }
        }
        DestroyObject();
    }
}
