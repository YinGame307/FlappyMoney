using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;

public class NapThe : MonoBehaviour {
	public InputField seri;
	public InputField pin;
	public GameObject topupBtn;
	public Dropdown providerValue;
	public string providerStr;
	public Dictionary<int, string> providerList = new Dictionary<int, string> ();
	public bool isDone;
	public RequestStatus requestStatus;
	// Use this for initialization
	void OnEnable(){
		seri.text = "";
		pin.text = "";
	
		providerValue.value = 0;
		checkContent ();
	}

	void Start () {
		providerList.Add (0, "VIETTEL");
		providerList.Add (1, "VNP");
		providerList.Add (2, "VMS");
	}
	
	public void checkContent(){
		if (seri.text.Equals ("") || pin.text.Equals ("")) {
			topupBtn.SetActive (false);
		} else {
			topupBtn.SetActive (true);
		}
	}

	public void setProviderStr(int value){
		providerStr = providerList [value];

	}

	public void topUp(){
		requestStatus.gameObject.SetActive (true);
		requestStatus.setStatusString ("Đang thực hiện ...");
		requestStatus.tryAgain = topUp;

		User user = Player.user;
		PostData data = new PostData ("card_charge");
		data.data.Add ("uid", user.id);
		data.data.Add ("user_name", user.name);
		data.data.Add ("pin_card", pin.text);
		data.data.Add ("card_serial", seri.text);
		data.data.Add ("card_type", providerStr);

		string vf = "";
		string source = user.userName + "" + pin.text + "" + seri.text + "" + providerStr;
		source += "3780005cf30852b977ea7e6c2e95aac2";
		using (MD5 md5Hash = MD5.Create())
		{
			vf = GetMd5Hash(md5Hash, source);

		}

		data.data.Add ("vf", vf);

		StartCoroutine (doAPI (data.toString (), onSuccess, onFail));
		StartCoroutine (timeOut (onTimeOut));
	}

	static string GetMd5Hash(MD5 md5Hash, string input)
	{

		// Convert the input string to a byte array and compute the hash.
		byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

		// Create a new Stringbuilder to collect the bytes
		// and create a string.
		StringBuilder sBuilder = new StringBuilder();

		// Loop through each byte of the hashed data 
		// and format each one as a hexadecimal string.
		for (int i = 0; i < data.Length; i++)
		{
			sBuilder.Append(data[i].ToString("x2"));
		}

		// Return the hexadecimal string.
		return sBuilder.ToString();
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

	void onSuccess(string res){
		try{
			JsonData data = JsonMapper.ToObject(res);
			if(bool.Parse(data["status"].ToString()) && data["message"].ToString().Equals("success")){
				requestStatus.onSuccess ("Nạp thành công !");
				int pending_coin = int.Parse(data["data"]["pending_coin"].ToString());
				int active_coin = int.Parse(data["data"]["available_coin"].ToString());
				Player.user.pendingCoin = pending_coin;
				Player.user.avaiableCoin = active_coin;
			}else{
				onFail("");
			}
		}catch(System.Exception ex){
			onFail("");
		}

	}

	void onFail(string res){
		requestStatus.onFail ("Nạp không thành công !");
	}

	void onTimeOut(string res){
		onFail ("");
	}

	IEnumerator timeOut(Action<string> onTimeOut){
		yield return new WaitForSeconds (10);
		if (!isDone) {
			if (onTimeOut != null) {
				onTimeOut ("");
			}
		}
	}
		
}

