using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Y_RectPositionTween : Y_Tween {

	public override IEnumerator doPlay ()
	{
		yield return new WaitForSeconds (deplay);
		GetComponent<RectTransform> ().DOAnchorPos (end, duration);
	}

	public override void resetToBegin ()
	{
		
	}
}
