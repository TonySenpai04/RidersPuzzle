using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObjectHandler 
{
    public void ClearHiddenObjects(Dictionary<Vector2Int, GameObject> hiddenObjectInstances,GridController gridController)
    {
        foreach (var hiddenObject in hiddenObjectInstances.Values)
        {
            if (hiddenObject != null && hiddenObject.activeSelf) 
            {
                hiddenObject.GetComponent<HiddenObject>().DestroyObject();
            }
            else if (hiddenObject != null) 
            {
                Object.Destroy(hiddenObject);
            }
        }
        gridController.StopAllCoroutines();
        hiddenObjectInstances.Clear();
    }
    public void ClearHiddenObjectsNoCroutine(Dictionary<Vector2Int, GameObject> hiddenObjectInstances, GridController gridController)
    {
        foreach (var hiddenObject in hiddenObjectInstances.Values)
        {

            if (hiddenObject != null)
            {
                Object.Destroy(hiddenObject);
            }

        }
        gridController.StopAllCoroutines();
        hiddenObjectInstances.Clear();
    }
    public void LoadHiddenObjects(Level level, GridController gridController, bool isActive,
        Dictionary<Vector2Int, GameObject> hiddenObjectInstances)
    {
        ClearHiddenObjects( hiddenObjectInstances,gridController);
        float screenWidth = Camera.main.orthographicSize * 2 * 9f / 16f; 
        float cellSize =(float) (screenWidth - 0.1 * (6 - 1)) / 6-0.1f;

        foreach (var hiddenObjectInfo in level.hiddenObjects)
        {
            Vector2Int positionKey = new Vector2Int(hiddenObjectInfo.row, hiddenObjectInfo.col);
            GameObject cell = gridController.grid[hiddenObjectInfo.row, hiddenObjectInfo.col];
            GameObject hiddenObject = Object.Instantiate(hiddenObjectInfo.objectPrefab, cell.transform.position, Quaternion.identity);
            hiddenObject.transform.localScale = new Vector3(cellSize, cellSize, 1);
            hiddenObject.transform.SetParent(cell.transform);
            hiddenObject.SetActive(isActive);
            hiddenObjectInstances[positionKey] = hiddenObject;
            gridController.StartCoroutine(AnimateScale(hiddenObject, 0.5f));
        }
    }
    private IEnumerator AnimateScale(GameObject obj, float duration)
    {
        Vector3 initialScale = Vector3.zero; 
        Vector3 targetScale = obj.transform.localScale; 
        float elapsedTime = 0f;

        obj.transform.localScale = initialScale; 

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            if (obj != null)
            {
                obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
            }
            yield return null;
        }

        obj.transform.localScale = targetScale; 
    }

    public GameObject GetHiddenObject(Vector2Int position,
        Dictionary<Vector2Int, GameObject> hiddenObjectInstances)
    {
        hiddenObjectInstances.TryGetValue(position, out var obj);
        return obj;
    }
    public List<GameObject> GetHiddenObjectsInRow(int row, Dictionary<Vector2Int, GameObject> hiddenObjectInstances)
    {
        List<GameObject> objectsInRow = new List<GameObject>();

        foreach (var entry in hiddenObjectInstances)
        {
            if (entry.Key.x == row)
            {
                objectsInRow.Add(entry.Value);
            }
        }

        return objectsInRow;
    }
    public List<GameObject> GetHiddenObjectsInColumn(int col, Dictionary<Vector2Int, GameObject> hiddenObjectInstances)
    {
        List<GameObject> objectsInColumn = new List<GameObject>();

        foreach (var entry in hiddenObjectInstances)
        {
            if (entry.Key.y == col)
            {
                objectsInColumn.Add(entry.Value);
            }
        }

        return objectsInColumn;
    }
   

    public List<GameObject> GetHiddenObjectsAround(int row, int col, Dictionary<Vector2Int, GameObject> hiddenObjectInstances)
    {
        List<GameObject> objectsAround = new List<GameObject>();

        Vector2Int[] directions = {
        new Vector2Int(-1, 0), // Ô trên
        new Vector2Int(1, 0),  // Ô dưới
        new Vector2Int(0, -1), // Ô bên trái
        new Vector2Int(0, 1)   // Ô bên phải
    };

        foreach (var dir in directions)
        {
            int newRow = row + dir.x;
            int newCol = col + dir.y;
            Vector2Int positionKey = new Vector2Int(newRow, newCol);
            if (hiddenObjectInstances.ContainsKey(positionKey))
            {
                objectsAround.Add(hiddenObjectInstances[positionKey]);
            }
        }

        return objectsAround;
    }
}
