using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelConfig : MonoBehaviour {
	public bool isTalking;

	public Image avatarImage;
	public Image textBG;
	public Text dialogue;
	public string currentText;
	private bool dialogueStarted;
	private Coroutine dialogueCoroutine;
	[SerializeField] private RectTransform imageTransform;
	[SerializeField] private GameObject nameObject;
	[SerializeField] private Text characterName;
	[SerializeField] private bool isLeft;

	public string path;
	public float debugTime;

	private Color maskActiveColor = new Color(103.0f/255.0f, 101.0f/255.0f, 101.0f/255.0f);


	public void Configure (Dialogue currentDialogue) {

		if (isLeft) {
			isTalking = currentDialogue.currentSpeaker == "left";
			avatarImage.sprite = LoadSprite(currentDialogue.leftSprite);
			FlipAvatarImage(currentDialogue.leftSpriteFlipped);

			if (currentDialogue.currentSpeaker == "left") {
				characterName.text = currentDialogue.name;
				nameObject.SetActive(true);
			} else {
				nameObject.SetActive(false);
			} 

		} else {
			isTalking = currentDialogue.currentSpeaker == "right";
			avatarImage.sprite = LoadSprite(currentDialogue.rightSprite);
			FlipAvatarImage(currentDialogue.rightSpriteFlipped);

			if (currentDialogue.currentSpeaker == "right") {
				characterName.text = currentDialogue.name;
				nameObject.SetActive(true);
			} else {
				nameObject.SetActive(false);
			} 
		}

		DetermineCharacterMask();
		imageTransform = avatarImage.GetComponent<RectTransform>();
	}

	public void DetermineCharacterMask () {
		if (isTalking) {
			avatarImage.color = Color.white;		
		} else {
			avatarImage.color = maskActiveColor;			
		}
	}

	public void SetCharacterMask (bool toggle) {
		if (toggle) {
			avatarImage.color = Color.white;
		} else {
			avatarImage.color = maskActiveColor;
		}
	}


	private void FlipAvatarImage (bool leftFacing) {
		if (leftFacing) {
            imageTransform.localRotation = Quaternion.Euler(0, 0, 0);
		} else {
			imageTransform.localRotation = Quaternion.Euler(0, 180, 0);
		}
	}

	//a shortened method call for convenience
	private Sprite LoadSprite (string spriteName) {
		return MasterManager.atlasManager.LoadSprite(spriteName);
	}

	public void Reset () {
		avatarImage.sprite = null;
		characterName.text = "";
	}

	public void SetTextColor (Color color) {
		nameObject.GetComponent<Text>().color = color;
	}
}