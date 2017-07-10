using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBaseHandler : MonoBehaviour {
	public Text log;

	public void Start() {

		//log.text += "====" + IsPlayServicesAvailable ();
		Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
		Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
	}

	public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
	  	UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
		Player.ins.setFCMToken (token.Token);
		//log.text += "Received Registration Token: " + token.Token;
	}

	public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
		UnityEngine.Debug.Log ("Received a new message from: " + e.Message);
		//log.text += "Received a new message from: " + e.Message.ToString();
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
