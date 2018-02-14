using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AtlasManager))]
[RequireComponent(typeof(AnimationManager))]
[RequireComponent(typeof(DialogueManager))]
[RequireComponent(typeof(PanelManager))]
[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(SoundManager))]


public class MasterManager : MonoBehaviour {

	private List<IManager> _managerList = new List<IManager>();

	public static AtlasManager atlasManager { get; private set; }
	public static AnimationManager animationManager { get; private set; }
	public static DialogueManager dialogueManager { get; private set; }
	public static PanelManager panelManager { get; private set; }
	public static GameManager gameManager { get; private set; }
	public static SoundManager soundManager { get; private set; }

	void Awake () {
		atlasManager = GetComponent<AtlasManager>();
		animationManager = GetComponent<AnimationManager>();
		dialogueManager = GetComponent<DialogueManager>();
		panelManager = GetComponent<PanelManager>();
		gameManager = GetComponent<GameManager>();
		soundManager = GetComponent<SoundManager>();
		_managerList.Add(animationManager);
		_managerList.Add(atlasManager);
		_managerList.Add(dialogueManager);
		_managerList.Add(panelManager);
		_managerList.Add(gameManager);
		_managerList.Add(soundManager);

		StartCoroutine (BootAllManagers());
	}

	private IEnumerator BootAllManagers () {
		foreach (IManager manager in _managerList) {
			manager.BootSequence();
		}

		yield return null;
	}
}



