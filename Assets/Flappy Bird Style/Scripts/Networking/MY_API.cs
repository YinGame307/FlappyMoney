using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MY_API : MonoBehaviour{
	public Action<string> onSuccess;
	public Action<string> onFailed;
	public string data;
	public Action<string> onTimeOut;
	private bool isDone;

	public MY_API(){

	}
	public MY_API(string data, Action<string> onSuccess, Action<string> onFailed, Action<string> onTimeOut){
		this.onSuccess = onSuccess;
		this.onFailed = onFailed;
		this.onTimeOut = onTimeOut;
		this.data = data;
	}

	public void callApi(){
		StartCoroutine (doAPI ());
	}

	public IEnumerator doAPI(){
		Debug.Log ("aaaaaaaaaaaaaaaaa");
		yield return null;

		//		//loading.SetActive (false);
		//		UnityWebRequest request = new UnityWebRequest("http://api.flappymoney.com:8080", "POST");
		//		byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(this.data);
		//		request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
		//		request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		//		request.SetRequestHeader("Content-Type", "application/json");
		//
		//		yield return request.Send();
		//
		//		if (request.isError) {
		//			if (this.onFailed != null) {
		//				onFailed ("");
		//			}
		//		} else {
		//			if (this.onSuccess != null) {
		//				this.onSuccess (request.downloadHandler.text);
		//			}
		//		}
		//		isDone = true;
	}

	IEnumerator timeOut(){
		yield return new WaitForSeconds (10);
		if (!isDone) {
			if (this.onTimeOut != null) {
				onTimeOut ("");
			}
		}
	}
}