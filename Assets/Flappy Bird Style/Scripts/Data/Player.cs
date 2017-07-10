using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
	public static bool loged;
	public static Player ins;
	public static User user;
	//public static bool canErnMoney;
	public static int secPlayed;

	public GameObject loginPopup;
	public UIController uiController;
	public GameObject moneyOn;
	public GameObject moneyOff;
	public InboxManager im;

	void Awake(){
		//PlayerPrefs.DeleteAll ();
		if (PlayerPrefs.HasKey ("LASTTIME")) {
			DateTime now = DateTime.Now;
			string nowStr = now.Year + "" + now.Month + "" + now.Day;
			if (!PlayerPrefs.GetString ("LASTTIME").Equals (nowStr)) {
				PlayerPrefs.SetInt ("CANERNMONEY", 0);
				PlayerPrefs.SetString ("LASTTIME", nowStr);
			}
		} else {
			DateTime now = DateTime.Now;
			string nowStr = now.Year + "" + now.Month + "" + now.Day;
			PlayerPrefs.SetInt ("CANERNMONEY", 0);
			PlayerPrefs.SetString ("LASTTIME", nowStr);
		}

		//PlayerPrefs.DeleteAll ();
		ins = this;
		if (user == null) {
			user = new User ();
		}

		if (moneyOn != null && moneyOff != null) {
			moneyOn.SetActive (PlayerPrefs.GetInt ("CANERNMONEY") == 1);
			moneyOff.SetActive (PlayerPrefs.GetInt ("CANERNMONEY") == 0);
		}
	}

	void Start(){
		if (SceneManager.GetActiveScene().name.Equals("Main")) {
			InvokeRepeating ("countTime", 1, 1);
		}
	}

	void countTime(){
		secPlayed++;
		if (secPlayed >= 60) {			
			CancelInvoke ("countTime");
			PlayerPrefs.SetInt ("CANERNMONEY", 1);
			if (moneyOn != null && moneyOff != null) {
				moneyOn.SetActive (PlayerPrefs.GetInt ("CANERNMONEY") == 1);
				moneyOff.SetActive (PlayerPrefs.GetInt ("CANERNMONEY") == 0);
			}
		}
	}

	public void setFCMToken(string token){
		user.FCM_Token = token;
		if (user.id != null && user.secretKey != null && !user.id.Equals ("") && !user.secretKey.Equals ("")) {
			updateFCMToken ();
		}
	}

	public void firstLogin(){		
		
		Debug.Log (user);
		if (user.userName.Equals ("") || user.name.Equals ("") || user.token.Equals ("")) {
			return;
		}

		PostData data = new PostData ("login");
		data.data.Add ("username", user.userName);
		data.data.Add ("name", user.name);
		data.data.Add ("fb_token", user.token);

		StartCoroutine (doAPI (data.toString (), onLoginSuccess, onLoginFailed));
	}

	public void autoLogin(){
		loginPopup.SetActive (true);
		loginPopup.GetComponent<RequestStatus> ().tryAgain = autoLogin;
		loginPopup.GetComponent<RequestStatus> ().setStatusString("Đang đăng nhập....");

		user = LitJson.JsonMapper.ToObject<User> (PlayerPrefs.GetString ("USER"));
		PostData data = new PostData ("login");
		data.data.Add ("id", user.id);
		data.data.Add ("secret_key", user.secretKey);
		Debug.Log (data.toString ());
		StartCoroutine (doAPI (data.toString (), onLoginSuccess, onLoginFailed));
	}

	void onLoginSuccess(string res){
		Debug.Log (res);
		LoginResult lr = LitJson.JsonMapper.ToObject<LoginResult> (res);
		user.id = lr.data.info.id;
		user.secretKey = lr.data.info.secret_key;
		user.userGroup = lr.data.info.user_group;
		user.userLevel = lr.data.info.user_level;
		user.avaiableCoin = lr.data.info.available_coin;
		user.pendingCoin = lr.data.info.pending_coin;
		user.createdTime = lr.data.info.created_time;
		user.updateTime = lr.data.info.update_time;
		loginPopup.GetComponent<RequestStatus> ().onSuccess ("Đăng nhập thành công !!!");
		im.refresh ();
		//Update FCM token=========================================================
		updateFCMToken ();

		PlayerPrefs.SetString ("USER", LitJson.JsonMapper.ToJson (user));
		Debug.Log ( LitJson.JsonMapper.ToJson (user));
		uiController.logonScreen.SetActive (true);
		uiController.firstScreen.SetActive (false);
		uiController.notificationNumber.text = lr.data.getUnreadNotifi ().ToString ();
		loged = true;
	}

	void onLoginFailed(string res){
		Debug.Log (res);
		loginPopup.GetComponent<RequestStatus> ().onFail ("Đăng nhập thất bại !");
	}

	public void addCoin(){
		PostData data = new PostData ("add-coin");
		data.data.Add ("uid", user.id);
		data.data.Add ("timestamp", DateTime.Now.Ticks);
		data.data.Add ("did", SystemInfo.deviceUniqueIdentifier.ToString ());
		string key = "";
		string source = user.id + "" + DateTime.Now.Ticks.ToString() + SystemInfo.deviceUniqueIdentifier.ToString ();
		source += "974f30118509481dd3fdab9184a3234b";
        using (MD5 md5Hash = MD5.Create())
        {
            key = GetMd5Hash(md5Hash, source);
			//Debug.Log("The MD5 hash of " + source + " is: " + key + ".");		
        }
		data.data.Add ("key", key);
		StartCoroutine (doAPI (data.toString(), null, null));
	}

	public void updateFCMToken(){
		PostData data = new PostData ("update-fcm");
		data.data.Add ("uid", user.id);
		data.data.Add ("secret_key", user.secretKey);
		data.data.Add ("fcm_token", user.FCM_Token);
		StartCoroutine (doAPI (data.toString(), null, null));
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

	public void playGame(){
		Application.LoadLevel (1);
	}

}
