using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NhanVat : MonoBehaviour {
	public Image[] avatars;
	public Text levelStrInTitle;
	public Text levelStr;
	public Text name;
	string[] levelStrs = new string[6]{"GIÁP VẢI", "GIÁP ĐỒNG", "GIÁP VÀNG", "GIÁP BẠCH KIM", "GIÁP KIM CƯƠNG", "GIÁP QUÝ TỘC"};
	void OnEnable(){
		foreach (Image ava in avatars) {
			ava.enabled = false;
		}

		avatars [Player.user.userLevel - 1].enabled = true;
		levelStrInTitle.text = levelStrs [Player.user.userLevel - 1].ToString ();
		levelStr.text = levelStrs [Player.user.userLevel - 1].ToString ();
		name.text = Player.user.name;
	}
}
