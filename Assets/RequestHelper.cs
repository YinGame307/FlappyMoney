using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class RequestHelper : MonoBehaviour {

	public GameObject requestStt;
	
	public void requestWithDraw(){
		PostData data = new PostData ("request-withdraw");
		data.data.Add ("uid", Player.user.id);
		data.data.Add ("secret_key", Player.user.secretKey);
		requestStt.SetActive (true);
		requestStt.GetComponent<RequestStatus> ().setStatusString ("Đang yêu cầu ...");
		requestStt.GetComponent<RequestStatus> ().tryAgain = requestWithDraw;
		StartCoroutine (doAPI (data.toString (), onRequestSuccess, onRequestFail));

	}

	void onRequestSuccess(string res){
		requestStt.GetComponent<RequestStatus> ().onSuccess ("Yêu cầu thành công !");
	}

	void onRequestFail(string res){
		requestStt.GetComponent<RequestStatus> ().onSuccess ("Thất bại !");
	}

	IEnumerator doAPI(string data, Action<string> onSuccess, Action<string> onFailed){

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
