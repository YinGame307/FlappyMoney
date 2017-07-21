using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public abstract class Y_Tween : MonoBehaviour , Y_ITween
{
	public enum PlayType{		
		Single = 0, 
		Loop,
		PingPong,
	}
	public Vector3 start;
	public Vector3 end;
	public float duration;
	public float deplay;
	public PlayType playType;
	public UnityEvent onComplete;
	public UnityEvent onUpdate;

	public bool playOnEnable;

	void OnEnable(){
		if (playOnEnable) {
			play ();
		}
	}

	public void play (){
		onComplete.RemoveListener (play);
		onComplete.RemoveListener (resetToBegin);
		if (playType != PlayType.Single) {
			onComplete.AddListener (play);
		}

		if (playType == PlayType.Loop) {
			onComplete.AddListener (resetToBegin);
		}

		if (playType == PlayType.PingPong) {
			Vector3 temp = start;
			start = end;
			end = temp;
		}

		StartCoroutine (doPlay ());
	}
	public abstract void resetToBegin ();
	public abstract IEnumerator doPlay ();
}


