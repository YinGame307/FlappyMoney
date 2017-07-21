using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {
	public Sprite on;
	public Sprite off;

	public Image soundImg;
	public Image musicImg;
	public SoundManager sm;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("ISMUTESOUND") == 0) {			
			soundImg.sprite = on;
		} else {
			soundImg.sprite = off;
		}
		if (PlayerPrefs.GetInt ("ISMUTEBG") == 0) {			
			musicImg.sprite = on;
		} else {
			musicImg.sprite = off;
		}
	}
	
	public void soundOnClick(){
		if (PlayerPrefs.GetInt ("ISMUTESOUND") == 0) {
			PlayerPrefs.SetInt ("ISMUTESOUND", 1);
			soundImg.sprite = off;
		} else {
			PlayerPrefs.SetInt ("ISMUTESOUND", 0);
			soundImg.sprite = on;
		}
	}

	public void musicOnClick(){
		if (PlayerPrefs.GetInt ("ISMUTEBG") == 0) {
			PlayerPrefs.SetInt ("ISMUTEBG", 1);
			musicImg.sprite = off;
		} else {
			PlayerPrefs.SetInt ("ISMUTEBG", 0);
			musicImg.sprite = on;
		}
		if (sm != null) {
			sm.setBG ();
		}
	}
}
