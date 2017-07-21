using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Y_PositionTween : Y_Tween {	

	public override IEnumerator doPlay(){
		transform.position = start;
		yield return new WaitForSeconds (deplay);
		transform.DOMove (end, duration).OnComplete (() => {
			if(onComplete != null){
				onComplete.Invoke ();
			}
		}).OnUpdate (() => {
			if(onUpdate!=null){
				onUpdate.Invoke();
			}
		});
	}

	public override void resetToBegin(){
		transform.position = start;
	}
	 
}
