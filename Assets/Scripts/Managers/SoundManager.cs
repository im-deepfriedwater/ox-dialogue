using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, IManager {
	[SerializeField] private AudioSource dialogueSource;
	[SerializeField] private AudioSource musicSource;

	private const float defaultVolume = 0.1f;
	public ManagerState currentState {get; private set;}

	// Use this for initialization
	public void BootSequence () {
		// TO DO recode the basic boot sequence
	}
	
	public void PlayDialogueSound (AudioClip clip, float volume = defaultVolume) {
		dialogueSource.clip = clip;
		dialogueSource.volume = volume;
		dialogueSource.Play();
	}

	public void StopDialogueSound () {
		dialogueSource.Stop();
	}

	public void PlaySong (AudioClip song, bool loop = false, float volume = defaultVolume) {
		musicSource.loop = loop;
		musicSource.volume = volume;
		musicSource.clip = song;
		musicSource.Play();
	}

	void Start () {
		PlaySong(MasterManager.atlasManager.LoadSong("GM"), true, 0.3f);
	}


}
