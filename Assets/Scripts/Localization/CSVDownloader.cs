//using System.Collections;
//using UnityEngine;
//using UnityEngine.Networking;

//public class CSVDownloader
//{
//    private const string googleSheetID = "1-hxsybHHPtb7SZc292yI0qR6UNq_H0iTcllLzR6LOco";
//    private const string googleSheetGrid = "&gid=1779485847";

//    private const string url = "https://docs.google.com/spreadsheets/d/" + googleSheetID + "/export?format=csv" + googleSheetGrid;

//    public static IEnumerator DownloadData(System.Action<string> onCompleted)
//    {
//        yield return new WaitForEndOfFrame();

//        string downloadData = null;
//        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
//        {
//            Debug.Log("Starting Download...");
//            yield return webRequest.SendWebRequest();
//            int equalsIndex = ExtractEqualsIndex(webRequest.downloadHandler);
//            if (webRequest.isNetworkError || (-1 == equalsIndex))
//            {
//                Debug.Log("...Download Error: " + webRequest.error);
//                downloadData = PlayerPrefs.GetString("LastDataDownloaded", null);
//                string versionText = PlayerPrefs.GetString("LastDataDownloadedVersion", null);
//                Debug.Log("Using stale data version: " + versionText);
//            }
//            else
//            {
//                string versionText = webRequest.downloadHandler.text.Substring(0, equalsIndex);
//                int a = int.Parse(versionText);
//                int b = int.Parse(PlayerPrefs.GetString("LastDataDownloadedVersion"));
//                if (a != b)
//                {
//                    downloadData = webRequest.downloadHandler.text.Substring(equalsIndex + 1);
//                    PlayerPrefs.SetString("LastDataDownloadedVersion", versionText);
//                    PlayerPrefs.SetString("LastDataDownloaded", downloadData);
//                    Debug.Log("...Downloaded version: " + versionText);
//                }
//                else
//                {
//                    //downloadData = PlayerPrefs.GetString("LastDataDownloaded", null);
//                    //versionText = PlayerPrefs.GetString("LastDataDownloadedVersion", null);
//                    Debug.Log("Using stale data version: " + versionText);
//                }


//            }
//        }

//        onCompleted(downloadData);
//    }

//    private static int ExtractEqualsIndex(DownloadHandler d)
//    {
//        if (d.text == null || d.text.Length < 8)
//        {
//            return -1;
//        }

//        string versionSection = d.text.Substring(0, 5);
//        int equalsIndex = versionSection.IndexOf('=');
//        if (equalsIndex == -1)
//            Debug.Log("Could not find a '=' in the CVS");
//        return equalsIndex;
//    }
//}