using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopCoinUI : MonoBehaviour {
	
	public Text order;
	public Text name;
	public Text coin;
	public Text rank;
	public Image rankIcon;
	public Sprite oneSt;
	public Sprite twoNd;
	public Sprite threeTh;
	public void setUI(int od, string name, string coin, string rank){
		this.order.text = od.ToString ();
		this.name.text = name.ToString ();
		this.coin.text = coin.ToString ();
		this.rank.text = rank.ToString ();
		if (od % 2 == 0) {
			GetComponent<Image> ().color = Color.white;
		} else {
			GetComponent<Image> ().color = Color.gray;
		}

		if (od == 1) {
			rankIcon.sprite = oneSt;
		}
		if (od == 2) {
			rankIcon.sprite = twoNd;
		}
		if (od == 3) {
			rankIcon.sprite = threeTh;
		}
		if (od > 3) {
			rankIcon.enabled = false;
		}
	}

}
