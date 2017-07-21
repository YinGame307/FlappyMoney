using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using LitJson;

public class TopCoin : MonoBehaviour {
	public class TopRes{
		public bool status;
		public string message;
		public List<JsonData> data;
		public TopRes(){
			
		}
	}

	public GameObject topItem;
	public bool loaded;
	public string typeTop;
	public TopRes topRes;
	public Transform holder;
	public GameObject loading;
	// Use this for initialization
	void OnEnable () {
		if (!loaded) {
			getTop ();
		}
	}

	public void getTop()
	{
		PostData data = new PostData (typeTop);
		data.data.Add ("limit", 8);

		StartCoroutine (doAPI (data.toString(), onSuccess, null));
	}

	public void onSuccess(string res){
		
		try{
			topRes = JsonMapper.ToObject<TopRes>(res);
			for(int i = 0; i < topRes.data.Count; i ++){
				
				GameObject item = Instantiate(topItem, transform.position, transform.rotation);
				item.transform.SetParent(holder, false);
				item.GetComponent<TopCoinUI>().setUI(i + 1, topRes.data[i]["name"].ToString(), topRes.data[i]["coin"].ToString(), (i + 1).ToString());
			}
			loaded = true;
			loading.SetActive(false);
		}catch(System.Exception ex){
			Debug.LogError (ex.Message);
		}
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
