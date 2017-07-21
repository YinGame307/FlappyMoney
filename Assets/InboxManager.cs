using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MessageRespone{
	public bool status;
	public string message;
	public Message[] data;
	public MessageRespone(){
		
	}

	public int getMessCount(){
		int c = 0;
		foreach (Message mess in data) {
			if (!mess.isRead ()) {
				c++;
			}
		}
		return c;
	}
}
public class InboxManager : MonoBehaviour {
	public MessageRespone mw;
	public GameObject loading;
	public List<MessageItemUI> miUIs = new List<MessageItemUI>();

	public Transform messRoot;

	public GameObject messUiPrebf;

	public GameObject messReadPop;

	public GameObject inboxBtn;
	public Text inboxCount;
	public GameObject notifiHolder;
	// Use this for initialization
	void OnEnable () {
		if (Player.loged) {
			refresh ();
		}
	}

	public void refresh(){
		foreach (Transform obj in messRoot) {
			Destroy (obj.gameObject);
		}

		getMessage ();
	}

	public void getMessage(){
		PostData data = new PostData ("get-notification");
		data.data.Add ("uid", Player.user.id);
		//data.data.Add ("uid", 1);
		data.data.Add ("secret_key", Player.user.secretKey);
		loading.SetActive (true);
		Debug.Log (data.toString());
		StartCoroutine (doAPI (data.toString (), onGetSuccess, onGetFail));
	}

	void onGetSuccess(string result){
		try{
			mw = LitJson.JsonMapper.ToObject<MessageRespone> (result);
			if(!mw.status){
				if (inboxBtn != null) {
					inboxBtn.SetActive (false);
				}
				return;
			}
			loading.SetActive (false);
			Debug.Log (result);
			if (mw.data != null) {
				for (int i = 0; i < mw.data.Length; i++) {
					GameObject newMess = Instantiate (messUiPrebf) as GameObject;
					newMess.transform.parent = messRoot;
					newMess.GetComponent<MessageItemUI> ().myMess = mw.data [i];
					newMess.GetComponent<MessageItemUI> ().setUI ();
					newMess.GetComponent<MessageItemUI> ().im = this;
				}
			}
			if (inboxBtn != null) {
				inboxBtn.SetActive (true);
			}
			if(mw.getMessCount() > 0){
				notifiHolder.SetActive(true);
				inboxCount.text = mw.getMessCount().ToString();
			}else{
				notifiHolder.SetActive(false);
			}
		}catch(System.Exception){
			if (inboxBtn != null) {
				inboxBtn.SetActive (false);
			}
		}

	}



	void onGetFail(string mess){
		
	}

	public void readMess(Message mess){
		messReadPop.GetComponent<MessageReadPopup> ().myMess = mess;
		messReadPop.SetActive (true);

		PostData data = new PostData ("mark-noti-read");
		data.data.Add ("id", mess.id);
		data.data.Add ("uid", Player.user.id);
		Debug.Log (data.toString());
		StartCoroutine (doAPI (data.toString (), onReadSuccess, null));
	}

	void onReadSuccess(string res){
		refresh ();
	}

	IEnumerator doAPI(string data, Action<string> onSuccess, Action<string> onFailed){
		loading.SetActive (false);
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
