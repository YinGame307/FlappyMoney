using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using LitJson;

public class FBHelper : MonoBehaviour {

	public static FBHelper ins;
	public Text log;

	private static string appLink;

	public static string accessToken;

	public GameObject loginFBBtn;
	public UIController uiController;
	public bool checkLogin;
	// Use this for initialization
	void Start ()
	{
		Debug.Log (checkLogin);
		if(checkLogin){
			if (!Player.loged) {
				if (PlayerPrefs.HasKey ("USER")) {
					Player.ins.autoLogin ();
					//loginFBBtn.SetActive (false);
					//return;
				} else {
					//loginFBBtn.SetActive (true);
				}
			} else {
				uiController.firstScreen.SetActive (false);
				uiController.logonScreen.SetActive (true);
			}
		}
		ins = this;
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			FB.ActivateApp();
			//loginFBBtn.SetActive (true);

		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}
	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	public void getMe()
	{
		Player.ins.loginPopup.SetActive (true);
		Player.ins.loginPopup.GetComponent<RequestStatus> ().tryAgain = login;
		Player.ins.loginPopup.GetComponent<RequestStatus> ().setStatusString("Đang đăng nhập....");
		FB.API("me/?fields=id,name", HttpMethod.GET, delegate (IGraphResult result) {
			JsonData data = JsonMapper.ToObject(result.RawResult);

			//FBData.ins.user = new User();
			Player.user.userName = data["id"].ToString();
			Player.user.name = data["name"].ToString();
			Player.ins.firstLogin();
		});
	}

	public void login()
	{
		var perms = new List<string>(){"public_profile", "email", "user_friends"};
		FB.LogInWithReadPermissions(perms, AuthCallback);
	}
	private void AuthCallback (ILoginResult result) {
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			accessToken = aToken.TokenString;
			Debug.Log (accessToken);
			Player.user.token = accessToken;
			loginFBBtn.SetActive (false);
			foreach (string perm in aToken.Permissions) {
				//Debug.Log(perm);
			}		
			getMe();
		} else {
			Debug.Log("User cancelled login");		
		}
	}
	public void share()
	{
		FB.ShareLink(
			new Uri("https://fb.me/235722536838729"),
			"Join FastRacing",
			"Download Now !",
			new Uri ("http://ads.haanhmedia.com/images/duaxe_icon.png"),
			callback: ShareCallback
		);
	}

	private void ShareCallback (IShareResult result) {
		if (result.Cancelled || !String.IsNullOrEmpty(result.Error)) {
			Debug.Log("ShareLink Error: "+result.Error);
		} else if (!String.IsNullOrEmpty(result.PostId)) {
			// Print post identifier of the shared content
			Debug.Log(result.PostId);
		} else {
			PlayerPrefs.SetInt ("CANERNMONEY", 1);
		}
	}
	public void getTotalFriend(bool installed)
	{

		FB.API("me?fields=friends", HttpMethod.GET,
			delegate (IGraphResult result) {
				int total = 0;
				JsonData data = JsonMapper.ToObject(result.RawResult);
				total = int.Parse((data["friends"]["summary"]["total_count"].ToString()));
				Debug.Log("total" + total);
				getListFriends(total, installed);
			});
	}
	public void getListFriends(int limit, bool installed)
	{
		string graph = "";
		if (installed) {
			graph = "me/?fields=friends.limit("+ limit.ToString() +"){id,name,picture.type(large)}";
		} else {
			graph = "me/?fields=invitable_friends.limit("+ limit.ToString() +"){id,name,picture.type(large)}";
		}


		FB.API(graph, HttpMethod.GET, delegate (IGraphResult result){
//			if (!string.IsNullOrEmpty (result.RawResult)) {
//				//Debug.Log (result.RawResult);
//				JsonData data = JsonMapper.ToObject(result.RawResult);
//				//Debug.Log(data.Keys.Contains);
//				if (data.Keys.Contains("friends")) {
//					for (int i = 0; i < data ["friends"] ["data"].Count; i++) {
////						User obj = new User ();
////						obj.id = data ["friends"] ["data"] [i] ["id"].ToString ();
////						obj.name = data ["friends"] ["data"] [i] ["name"].ToString ();
////						obj.profilePic = data ["friends"] ["data"] [i] ["picture"] ["data"] ["url"].ToString ();
////						//Debug.Log (obj.name);
////						FBData.Friends_installed.Add (obj);
//
//					}
//				}else if (data.Keys.Contains("invitable_friends")) {
//					for (int i = 0; i < data ["invitable_friends"] ["data"].Count; i++) {
////						User obj = new User ();
////						obj.id = data ["invitable_friends"] ["data"] [i] ["id"].ToString ();
////						obj.name = data ["invitable_friends"] ["data"] [i] ["name"].ToString ();
////						obj.profilePic = data ["invitable_friends"] ["data"] [i] ["picture"] ["data"] ["url"].ToString ();
////						//Debug.Log (obj.name);
////						FBData.Friends_invitable.Add (obj);
//
////					}
//				}
//				//Debug.Log (FBData.Friends_invitable.Count);
//			}
		});
	}



	void appLinkCallBack(IAppLinkResult result)
	{
		if (!string.IsNullOrEmpty (result.Url)) {
			appLink = result.Url;
			Debug.Log (appLink);
		}
	}
	private void ProfilePhotoCallback(IGraphResult result)
	{
		if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
		{
			//proPic. = result.Texture;
//			Rect rec  = new Rect(0,0,result.Texture.width, result.Texture.height);
//			proPic.GetComponent<UI2DSprite>().sprite2D = Sprite.Create(result.Texture, rec, new Vector2(0.5f, 0.5f), 100);
//			bigProPic.GetComponent<UI2DSprite>().sprite2D = Sprite.Create(result.Texture, rec, new Vector2(0.5f, 0.5f), 100);
		}


	}

	public void Invite ()
	{
		FB.Mobile.AppInvite (
			new Uri ("https://fb.me/235722536838729"),
			new Uri ("http://ads.haanhmedia.com/images/duaxe_icon.png"),
			AppInviteCallback
		);
	}

	void AppInviteCallback(IAppInviteResult result)
	{
		if (!string.IsNullOrEmpty(result.RawResult))
		{			
//			logaa.text = result.RawResult;
//			JsonData res = JsonMapper.ToObject (result.RawResult);
//			if (res.Keys.Contains ("did_complete") || res.Keys.Contains ("didComplete")) {
//				logaa.text += " 11111111111";
//					logaa.text += " 0000000000000";
//					PopUpReward pUR = popupReward.GetComponent<PopUpReward>();
//					pUR.amount = 200;
//					pUR.icon = "gold";
//					pUR.typeSave = Define.GOLD;
//					popupReward.SetActive (true);
//			}
			//Debug.Log( "Success Response:\n" + result.RawResult);

		}
	}
//	public void request()
//	{
//		FB.AppRequest (
//			"Come play this game",null,null,null,null,null,null,
//			delegate (IAppRequestResult result){
//				//log.text = result.RawResult;
//			}
//		);
//
//	}
	public void requestOneFriends(string token)
	{
		List<string> list = new List<string> ();
		list.Add (token);
		FB.AppRequest ("Come play game", list, null, null, null, null, null, delegate (IAppRequestResult result) {
			log.text = "aaaaaaaaaaaaa "+result.RawResult;

			Debug.Log(result.RawResult);
		});
	}

}
