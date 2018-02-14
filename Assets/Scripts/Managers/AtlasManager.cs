using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for accessing the sprite class

public class AtlasManager : MonoBehaviour, IManager {

  //    placeHolderArt.Add("HeroSurprised", "'o'");
  //    placeHolderArt.Add("HeroBored", "'-'");
  //    placeHolderArt.Add("HeroContemplative", "'.'?");
  //    placeHolderArt.Add("HeroLetDown", ";-;");

  //    placeHolderArt.Add("HeroineBored", "()'-')");
  //    placeHolderArt.Add("HeroineSurprised", "()'O')");
  //    placeHolderArt.Add("HeroineContemplative", "()'.')?");
  //    placeHolderArt.Add("HeroinePuzzled", "~()?.?)~");

    public static Dictionary<string, string> spriteDict = new Dictionary<string, string>{
    	{"HeroSurprised", "blu_surp"},
    	{"HeroBored", "blu_bored"},
    	{"HeroContemplative", "blu_confused"},
    	{"HeroLetDown", "blu_conflicted"},
    	{"DhaContemplative", "dha_bored"},
    	{"DhaPleased", "dha_smile"},
    	{"DhaScheming", "dha_creep"},
    	{"DhaSurp", "dha_surp"}
    };

    public static Dictionary<string, string> dialogueSounds = new Dictionary<string, string>{
    	{"Blu", "vocal_scatters/3a"},
    	{"Dha", "vocal_scatters/1c"}
    };

    public static Dictionary<string, string> songsDict = new Dictionary<string, string>{
    	{"GM", "Music/GreenMachine"},
      {"", ""}
    };

	public ManagerState currentState {get; private set;}

	public void BootSequence() {
		Debug.Log(string.Format("{0} is booting up", GetType ().Name));

		currentState = ManagerState.Completed;

		Debug.Log(string.Format("{0} status = {1}", GetType ().Name, currentState));
	}

	public Sprite LoadSprite(string resourceName) {
		if (spriteDict.ContainsKey(resourceName)) {
			return Resources.Load<Sprite>(spriteDict[resourceName]);
		}	

		return null;
	}

	public AudioClip LoadDialogueSound (string resourceName) {
		if (dialogueSounds.ContainsKey(resourceName)) {
			return Resources.Load<AudioClip>(dialogueSounds[resourceName]);
		}	

		return null;
	}

	public AudioClip LoadSong (string resourceName) {
	if (songsDict.ContainsKey(resourceName)) {
		return Resources.Load<AudioClip>(songsDict[resourceName]);
	}	

	return null;
}		
}
