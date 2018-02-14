using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeEvent {
	public List<Dialogue> dialogues;
}

public struct DialogueOutdated {
	public CharacterType characterType;
	public string name;
	public string atlasImageName;
	public string dialogueText;
	public bool leftFacing;
	public string position;
}

public enum CharacterType {
	Hero, Ally, Mentor
}