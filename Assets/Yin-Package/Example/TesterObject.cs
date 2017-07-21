using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterObject : MonoBehaviour {

	public void onCompleted(){
		Debug.Log ("Completed");
	}

	public void onUpdate(){
		Debug.Log (transform.position);
	}
}
