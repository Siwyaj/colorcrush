using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendToServer : MonoBehaviour
{
    private string webhookUrl = "https://webhook.site/2b571ca0-d99c-45b2-b5e3-f9a1e7ca300d";
    private static SendToServer _instance;
    private static SendToServer Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("SendToServer");
                _instance = go.AddComponent<SendToServer>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    public void LogColorTrial(string log)
    {
        string json = JsonUtility.ToJson(log);
        StartCoroutine(SendData(json));
    }


    private IEnumerator SendData(string json)
    {
        using (UnityWebRequest www = new UnityWebRequest(webhookUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending data: " + www.error);
            }
            else
            {
                Debug.Log("Data sent successfully!");
            }
        }
    }

}
