using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoiThe : MonoBehaviour {
	public Text activeCoin;

	void OnEnable(){
		activeCoin.text = Player.user.avaiableCoin.ToString ();
	}

}
