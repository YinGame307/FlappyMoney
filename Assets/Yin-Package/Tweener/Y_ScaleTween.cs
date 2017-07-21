using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
 

public class Y_ScaleTween : Y_Tween {
	
	public override void resetToBegin ()
	{
		transform.localScale = start;
	}

	public override IEnumerator doPlay ()
	{
		yield return new WaitForSeconds (deplay);
		transform.DOScale (end, duration).OnUpdate (() => {
			if (onUpdate != null) {
				onUpdate.Invoke ();
			}
		}).OnComplete (() => {
			if(onComplete != null){
				onComplete.Invoke();
			}
		});
	}
}
