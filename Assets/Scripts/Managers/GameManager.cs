using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IManager {
	public ManagerState currentState { get; private set; }
	public bool gamePaused { get; private set; }

	public void BootSequence () {
		Debug.Log (string.Format ("{0} is booting up.", GetType().Name));
		currentState = ManagerState.Completed;
		Debug.Log (string.Format ("{0} status is {1}", GetType().Name, currentState));
	}

	private void LoadCurrentLevelEvents (int levelID) {
		// TO-DO
	}

	//  meant for UI events such as dialogue
	public bool Pause () {
		if (!gamePaused) {
		    gamePaused = true;
		    Time.timeScale = 0.001f;
		} else {
			Debug.Log ("Game has already been paused by another process.");
			return false;
		}

		return true;
	}

	public bool Resume () {
		if (gamePaused) {
		    gamePaused = false;
		    Time.timeScale = 0.001f;
		} else {
			Debug.Log ("Game cannot be resumed as it is not paused.");
			return false;
		}

		return true;
	}

	public void TriggerEvent (int eventID) {
		switch (eventID) {
			case 0 :
				break;
			case 1 :
				break;
		}
	}

	public void TriggerDialogue (int conversationID) {
		var playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
		playerManager.InitializeDialogueControls();
		MasterManager.panelManager.InitializeConversation(conversationID);
	}

	public void ExitDialogue () {
		var playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
		playerManager.ExitDialogueControls();
	}
}
