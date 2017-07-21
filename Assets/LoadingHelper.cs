using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingHelper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("SHOWADS") == 1) {
			if (GoogleAdmob.interstitial.IsLoaded ()) {
				GoogleAdmob.admob.ShowInterstitial ();
			} else {
				SceneManager.LoadScene (2);
			}
		} else {
			SceneManager.LoadScene (2);
		}

	}
}
