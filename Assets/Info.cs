using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;


public class Info : MonoBehaviour {

	public class InfoRes
	{
		public bool status;
		public string message;
		public string data;

		public InfoRes(){
			
		}
	}

	public Text content;
	public string type;
	public bool loaded;

	void OnEnable () {
		if (!loaded) {
			getContent ();
		}
	}

	void getContent(){
		content.text = "Đang tải ...";
		PostData data = new PostData ("get-info");
		data.data.Add ("code", type);

		StartCoroutine (doAPI (data.toString(), onSuccess, null));
	}

	void onSuccess(string res){
		Debug.Log (res);
		try{
			InfoRes infoRes = LitJson.JsonMapper.ToObject<InfoRes>(res);
			if(infoRes.status){
				content.text = infoRes.data;
				Debug.Log(infoRes.data);
			}
		}catch(Exception ex){
			
		}
	}

	void onFail(string res){

	}


	IEnumerator doAPI(string data, Action<string> onSuccess, Action<string> onFailed){
		//loading.SetActive (false);
		UnityWebRequest request = new UnityWebRequest("http://api.flappymoney.com:8080", "POST");
		byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
		request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		yield return request.Send();

		if (request.isError) {
			if (onFailed != null) {
				onFailed ("");
			}
		} else {
			if (onSuccess != null) {
				onSuccess (request.downloadHandler.text);
			}
		}

	}
}
