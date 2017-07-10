using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageReadPopup : MonoBehaviour {

	public Text content;
	public Text title;
	public Message myMess;
		
	void OnEnable(){
		content.text = myMess.content;
		title.text = myMess.title;
	}
}
