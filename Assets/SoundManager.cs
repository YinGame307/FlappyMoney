using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public static SoundManager ins;
	public AudioSource bgMusic;
	public AudioClip flySound;
	public AudioClip btnClickSound;
	public AudioClip readySound;
	public AudioClip crashSound;
	public AudioClip notifiSound;
	public AudioClip dieSound;

	public AudioSource audioSource;
	// Use this for initialization
	void Awake(){
		ins = this;
	}
	void Start () {
		setBG ();
	}

	public void setBG(){
		if (PlayerPrefs.GetInt ("ISMUTEBG") == 0) {
			bgMusic.volume = 1;
		} else {
			bgMusic.volume = 0;
		}
	}

	public void playFlySound(){
		if (PlayerPrefs.GetInt ("ISMUTESOUND") == 0) {
			audioSource.PlayOneShot (flySound, 1);
		}
	}
	public void playBtnClick(){
		if (PlayerPrefs.GetInt ("ISMUTESOUND") == 0) {
			audioSource.PlayOneShot (btnClickSound, 1);
		}
	}
	public void playReady(){
		if (PlayerPrefs.GetInt ("ISMUTESOUND") == 0) {
			audioSource.PlayOneShot (readySound, 1);
		}
	}
	public void playCrash(){
		if (PlayerPrefs.GetInt ("ISMUTESOUND") == 0) {
			audioSource.PlayOneShot (crashSound, 1);
		}
	}
	public void playDie(){
		if (PlayerPrefs.GetInt ("ISMUTEBG") == 0) {
			audioSource.PlayOneShot (dieSound, 1);
			bgMusic.Stop ();
		}
	}
	public void playNotifi(){
		if (PlayerPrefs.GetInt ("ISMUTESOUND") == 0) {
			audioSource.PlayOneShot (notifiSound, 1);
		}
	}
}

