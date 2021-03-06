#region License
/*
 * Copyright 2021 AutoCore
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Authors: AutoCore Members
 *
 */
#endregion
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
        public Action<string> OnGetRequest;
        public Action<string> OnPostRequest;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(GetJsonData(new Uri(Path.Combine(Application.streamingAssetsPath, "AddressConfig.json"))));
        }

        public IEnumerator GetWebRequest()
        {
            Debug.Log(getAddress);
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
                    OnGetRequest.Invoke(webRequest.downloadHandler.text);
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
            Debug.Log(postAddress + "----" + content);

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
                OnPostRequest.Invoke(webRequest.downloadHandler.text);
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
                OnPostRequest.Invoke(webRequest.downloadHandler.text);
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }
}
