using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using XMLFactory;

public struct Dialogue {
    public int ID;
    public int jumpID;
    public string currentSpeaker;
    public string name;
    public string leftSprite;
    public bool leftSpriteFlipped;
    public string rightSprite;
    public bool rightSpriteFlipped;
    public string message;
    public List<Choice> choices;
    public bool hasChoice;
    public bool isEnd;
}

public struct Choice {
    public string message;
    public string command;
    public int pointValue;
    public int nextJumpID;
}

public class DialogueManager : MonoBehaviour, IManager {
	public ManagerState currentState { get; private set; }

    public int conversationID;
    private int currentDialogueID = -1;
    Dictionary<int, Dialogue> currentConversation = new Dictionary<int, Dialogue>();
    int neutralTotal {get; set;}
    int manipulativeTotal {get; set;}
    int positiveTotal {get; set;}
    public bool isEnd {get; private set;}
    public Dialogue currentDialogue;
    
    public DialogueManager () {
        neutralTotal = 0;
        manipulativeTotal = 0;
        positiveTotal = 0;
    }

    public void BootSequence () {
    	Debug.Log(string.Format ("{0} is booting up.", GetType().Name));
    	Debug.Log (string.Format ("{0} status = {1}", GetType().Name, currentState));
    }

    public void LoadDialogue (int conversationID){
        this.conversationID = conversationID;
        currentConversation = XMLAssembly.RunFactoryForConversation(conversationID);
        currentDialogue = currentConversation[0];
    }

    public void AdvanceDialogue () {
        if (currentConversation.ContainsKey(currentDialogue.jumpID)) {
            currentDialogue = currentConversation[currentDialogue.jumpID];
            currentDialogueID = currentDialogue.ID;
            isEnd = currentDialogue.isEnd;
        } else {
            throw new ArgumentOutOfRangeException("Tried to access non existent dialogue ID. /n Look at dialogue ID #" + currentDialogueID.ToString());
        }
    }

    public void ProcessDialogueCommand (Choice choice) {
        currentDialogue.jumpID = choice.nextJumpID;

        switch (choice.command) {
            case "AddToNeutralTotal":
                AddToNeutralTotal(choice.pointValue);
                break;
        }
    }

    public void AddToNeutralTotal (int points) {
        neutralTotal += points;
    }

    public void AddToManipulativeTotal (int points) {
        manipulativeTotal += points;
    }

    public void AddToPositiveTotal (int points) {
        positiveTotal += points;
    }
}
