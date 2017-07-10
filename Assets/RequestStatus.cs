using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class RequestStatus : MonoBehaviour {

	public Text status;
	public GameObject okBtn;
	public GameObject tryAgainBtn;

	public Action tryAgain;
	public GameObject cancelBtn;

	public void setStatusString(string text){
		status.text = text;
		okBtn.SetActive (false);
		tryAgainBtn.SetActive (false);
		cancelBtn.SetActive (false);
	}

	public void onSuccess(string content){
		status.text = content;
		okBtn.SetActive (true);
		cancelBtn.SetActive (false);
		tryAgainBtn.SetActive (false);
	}

	public void onFail(string content){
		status.text = content;
		tryAgainBtn.SetActive (true);
		cancelBtn.SetActive (true);
		tryAgainBtn.GetComponent<Button> ().onClick.RemoveAllListeners ();
		tryAgainBtn.GetComponent<Button> ().onClick.AddListener (() => {
			tryAgain();	
		});
	}
}
