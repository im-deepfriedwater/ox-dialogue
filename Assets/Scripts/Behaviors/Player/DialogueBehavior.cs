using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBehavior : AbstractBehavior {

	public bool confirmPressed = false;
	public bool upPressed = false;
	public bool downPressed = false;

	public void EnterDialogue () {
		confirmPressed = false;
		ToggleScripts(false);
	}

	// confirm is 0
	// up is 1
	// down is 2

	// Update is called once per frame
	void Update () {
		confirmPressed = inputState.GetButtonValue(inputButtons[0]);
		upPressed = inputState.GetButtonValue(inputButtons[1]);
		downPressed = inputState.GetButtonValue(inputButtons[2]);
	}

	private IEnumerator RevertGameplayBehaviors () {
		yield return new WaitForSeconds(1);
		ToggleScripts(true);
		confirmPressed = false;
		upPressed = false;
		downPressed = false;
	}

	public void ExitDialogue (){
		StartCoroutine(RevertGameplayBehaviors());
	}

	public float GetButtonHoldTime (int indexOfButton) {
		return inputState.GetButtonHoldTime(inputButtons[indexOfButton]);
	}


}
