using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Unity.VisualScripting;

public class WWW : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetRequest("http://localhost/fiszki/GetQuestions.php"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + webRequest.downloadHandler.text);
                    string jsonString = webRequest.downloadHandler.text;
                    Debug.Log(jsonString);
                    //RootObject root = JsonUtility.FromJson<RootObject>(jsonString);
                    /*foreach (Database question in root.data)
                    {
                        Debug.Log(question);
                    }*/
                    break;
            }
        }
    }
}