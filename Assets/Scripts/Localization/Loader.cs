//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//public enum Language
//{
//    English,
//    Vietnamese
//}
//public class Loader : MonoBehaviour
//{
//    private Dictionary<string, string> localizedTexts;
//    private List<string> richText;

//    private void Awake()
//    {
//        this.localizedTexts = new Dictionary<string, string>();
//    }
//    private void Start()
//    {
//        LoadLanguage();
//    }

//    public void LoadLanguage()
//    {
//        StartCoroutine(CSVDownloader.DownloadData(AfterDownload));
//    }

//    public void AfterDownload(string data)
//    {
//        if (null == data)
//        {
//            Debug.LogError("Was not able to download data or retrieve stale data.");
            
//        }
//        else
//        {
//            StartCoroutine(ProcessData(data, 1));
//        }
//    }
//    public IEnumerator ProcessData(string data, int Language)
//    {
//        yield return new WaitForEndOfFrame();
//        yield return new WaitForEndOfFrame();
//        yield return new WaitForEndOfFrame();

//        if (data != null)
//        {
//            var csvText = data.Trim().Replace("\r\n", "\n");
//            var lines = csvText.Split("\n");
//            foreach (var line in lines)
//            {
//                var segments = line.Split(';');
//                if (segments.Length > 0)
//                {
//                    var localizeOrigin = segments[2];
//                    var localizeText = segments[3];
//                    var fontName = segments[5];

                   
//                }
//            }
//        }
//        else
//        {
//            Debug.LogError("Data is null !");
//        }
//    }
//}
