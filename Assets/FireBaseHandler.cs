using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBaseHandler : MonoBehaviour {
	void Start() {
		Firebase.Messaging.FirebaseMessaging.Subscribe("/topics/news");
		Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
		Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
	}
	public GameObject gift;
	public InboxManager iM;

	public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
	  	UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
		Player.ins.setFCMToken (token.Token);	
		PlayerPrefs.SetString ("FCMTOKEN", token.Token);
	}

	public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
		UnityEngine.Debug.Log("Received a new message");
		if (e.Message.From.Length > 0)
			UnityEngine.Debug.Log("from: " + e.Message.From);
		if (e.Message.Data.Count > 0) {
			if (e.Message.Data.ContainsKey ("action")) {
				SoundManager.ins.playNotifi ();
				if (e.Message.Data ["action"].Equals ("received-coin")) {
					gift.SetActive (true);
					Time.timeScale = 0;
					if (gift != null) {
						gift.SetActive (true);
					}
				} else {
					iM.refresh ();
				}
			}
		}
	}

	 public int IsPlayServicesAvailable()
        {
            const string GoogleApiAvailability_Classname = 
                "com.google.android.gms.common.GoogleApiAvailability";
            AndroidJavaClass clazz = 
                new AndroidJavaClass(GoogleApiAvailability_Classname);
            AndroidJavaObject obj = 
                clazz.CallStatic<AndroidJavaObject>("getInstance");

            var androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = androidJC.GetStatic<AndroidJavaObject>("currentActivity");

            int value = obj.Call<int>("isGooglePlayServicesAvailable", activity);

            // result codes from https://developers.google.com/android/reference/com/google/android/gms/common/ConnectionResult

            // 0 == success
            // 1 == service_missing
            // 2 == update service required
            // 3 == service disabled
            // 18 == service updating
            // 9 == service invalid
            return value;
        }
}
