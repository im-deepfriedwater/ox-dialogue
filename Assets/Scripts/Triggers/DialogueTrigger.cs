using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Trigger {

	[SerializeField] private int dialogueID = 1;
	[SerializeField] private int eventID = 1;
	[SerializeField] private bool eventOccurs;
	[SerializeField] private bool eventAfter;
 	[SerializeField] private bool active = true;
 	[SerializeField] private bool activeOnlyOnce = true;

	override protected void OnTriggerEnter2D (Collider2D target) {
	    if (target.gameObject.tag == targetTag && active) {

	    	var gameManager = MasterManager.gameManager;

	    	if (eventOccurs && !eventAfter) {
	    		gameManager.TriggerEvent (eventID);
	    	}

	    	gameManager.TriggerDialogue (dialogueID);

	    	if (eventOccurs && eventAfter) {
	    		gameManager.TriggerEvent (eventID);
	    	}

	    	active = activeOnlyOnce ? false : true;
	    	
     	}
    }  
}
