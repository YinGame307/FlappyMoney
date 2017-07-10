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
		if (PlayerPrefs.GetInt ("ISMUTEBG") == 0) {
			bgMusic.volume = 1;
		} else {
			bgMusic.volume = 0;
		}
	}
	
	public void playFlySound(){
		audioSource.PlayOneShot (flySound, 1);
	}
	public void playBtnClick(){
		audioSource.PlayOneShot (btnClickSound, 1);
	}
	public void playReady(){
		audioSource.PlayOneShot (readySound, 1);
	}
	public void playCrash(){
		audioSource.PlayOneShot (crashSound, 1);
	}
	public void playDie(){
		audioSource.PlayOneShot (dieSound, 1);
	}
	public void playNotifi(){
		audioSource.PlayOneShot (notifiSound, 1);
	}
}

