using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Y_Action : MonoBehaviour {

	public UnityEvent onEnableEvents;
	public UnityEvent onStartEvents;
	public UnityEvent onDisableEvents;

	// Use this for initialization
	void Start () {
		if (onStartEvents != null) {
			onStartEvents.Invoke ();
		}
	}

	void OnEnable(){
		if (onEnableEvents != null) {
			onEnableEvents.Invoke ();
		}
	}

	void OnDisable(){
		if (onDisableEvents != null) {
			onDisableEvents.Invoke ();
		}
	}
}


