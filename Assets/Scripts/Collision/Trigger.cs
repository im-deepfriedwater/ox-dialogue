using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
	[SerializeField]
	protected string targetTag = "Player";
	protected int triggerID;

	protected virtual void OnTriggerEnter2D (Collider2D target) {
		if (target.gameObject.tag == targetTag) {
			OnCollect (target.gameObject);
			var gameManager = MasterManager.gameManager;
			gameManager.TriggerEvent (triggerID);
		}
	}


	//  potentially unneeded
	protected virtual void OnCollect (GameObject target) {
		// purposefully empty for future classes to override
	}
}