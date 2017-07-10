using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour {

	public Text level;
	public Text pendingCoin;
	public Text activeCoin;

	void OnEnable(){
		setUI (Player.user);
	}

	public void setUI(User user){
		level.text = user.userLevel.ToString ();
		pendingCoin.text = user.pendingCoin.ToString ();
		activeCoin.text = user.avaiableCoin.ToString ();
	}
}
