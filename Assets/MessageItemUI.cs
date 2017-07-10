using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageItemUI : MonoBehaviour {

	public Message myMess;
	public Text title;
	public GameObject readed;
	public GameObject unRead;
	public InboxManager im;

	void OnEnable(){
		
	}

	public void setUI(){
		Debug.Log (myMess.is_read);
		title.text = myMess.title;
		//readed.SetActive (myMess.isRead ());
		//unRead.SetActive (!myMess.isRead ());

	}

	public void onRead(){
		im.readMess (myMess);
		myMess.readMessage ();
		setUI ();
	}

}
