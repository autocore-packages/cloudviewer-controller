using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;

namespace Assets.Scripts
{
    public class WebRequestServer : MonoBehaviour
    {
        public string postAddress;
        public string getAddress;
        public Action<string> OngetRequest;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(GetJsonData(new Uri(Path.Combine(Application.streamingAssetsPath, "AddressConfig.json"))));
        }

        public IEnumerator GetWebRequest()
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(getAddress))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                }
                else
                {
                    Debug.Log(webRequest.downloadHandler.text);
                    OngetRequest.Invoke(webRequest.downloadHandler.text);
                }
            }
        }
        public IEnumerator GetJsonData(Uri uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                }
                else
                {
                    Debug.Log(webRequest.downloadHandler.text);
                    string jsonStr = webRequest.downloadHandler.text;
                    
                    CVManager.Instance.addressNode.Config = JsonConvert.DeserializeObject<AddressConfig>(jsonStr);
                }
            }
        }

        public IEnumerator PostWebRequest(TrafficLightData data)
        {
            if (postAddress == null)
            {
                Debug.LogError("uri is null");
                yield break;
            }
            string content = JsonConvert.SerializeObject(data);
            Debug.Log(postAddress+"----"+content);
            
            byte[] databyte = Encoding.UTF8.GetBytes(content);
            UnityWebRequest webRequest = new UnityWebRequest(postAddress, UnityWebRequest.kHttpVerbPOST);
            webRequest.uploadHandler = new UploadHandlerRaw(databyte);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            yield return webRequest.SendWebRequest();
            Debug.Log(webRequest.responseCode);
            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
        public IEnumerator PostWebRequest(string data)
        {
            if (postAddress == null)
            {
                Debug.LogError("uri is null");
                yield break;
            }
            byte[] databyte = Encoding.UTF8.GetBytes(data);
            UnityWebRequest webRequest = new UnityWebRequest(postAddress, UnityWebRequest.kHttpVerbPOST);
            webRequest.uploadHandler = new UploadHandlerRaw(databyte);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            yield return webRequest.SendWebRequest();
            Debug.Log(webRequest.responseCode);
            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                Debug.Log(webRequest.downloadedBytes);
            }
        }
    }
}
