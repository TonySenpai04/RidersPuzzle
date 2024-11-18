using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataLevel
{
    public int Stage;
    public int Active;
    public int Move;
    public List<int> CoordinatesID = new List<int>();
    public int Total;
    
}

public class Loader : MonoBehaviour
{
    public List<DataLevel> dataLevels ;
    public List<DataLevel> GetData()
    {
        return dataLevels;
    }



    private void Start()
    {
        Load();
    }

    public void Load()
    {
        StartCoroutine(CSVDownloader.DownloadData(AfterDownload));
    }

    public void AfterDownload(string data)
    {
        if (null == data)
        {
            Debug.LogError("Was not able to download data or retrieve stale data.");
            
        }
        else
        {
            StartCoroutine( ProcessData( data, AfterProcessData ));
        }
    }

    private void AfterProcessData(string errorMessage)
    {
        if (null != errorMessage)
        {
            Debug.LogError("Was not able to process data: " + errorMessage);   
        }
        else
        {

        }
    }

    public IEnumerator ProcessData(string data, System.Action<string> onCompleted)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        if (data != null)
        {
            var csvText = data.Trim().Replace("\r\n", "\n");
            var lines = csvText.Split("\n");
            for (int i = 1; i < lines.Length; i++)
            {
                var segments = lines[i].Split(',');


                if (segments.Length > 0)
                {
                    DataLevel datas = new DataLevel();

                    for (int j = 0; j < segments.Length; j++)
                    {
                        if(j == 0)
                        {
                            datas.Stage = int.Parse(segments[j]);
                        }
                        else if (j == 1)
                        {
                            datas.Active = int.Parse(segments[j]);
                        }
                        else if(j == 2)
                        {
                            datas.Move = int.Parse(segments[j]);
                        } 
                        else if(j >= 3 && j <= 32)
                        {
                            datas.CoordinatesID.Add(!string.IsNullOrWhiteSpace(segments[j]) ? int.Parse(segments[j]) : 0);
                        }
                        else if( j == 33)
                        {
                            datas.Total = int.Parse(segments[j]);
                        }
                        else
                        {
                            break;
                        }
                    }
                  
                    dataLevels.Add(datas);

                }
            }
        }
        else
        {
            Debug.LogError("Data is null !");
        }
        foreach (var item in dataLevels)
        {
            Debug.Log(item);
        }
        onCompleted(null);
    }

    
}
