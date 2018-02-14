using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PanelManager : MonoBehaviour, IManager {

	public ManagerState currentState { get; private set; }

	[SerializeField] private Image textBoxImage;
	[SerializeField] private bool dialogueFinished = false;
	[SerializeField] private Text dialogueText;
	[SerializeField] public int dialogueID { get; set; }

    [SerializeField] private GameObject finishIndicator;
	[SerializeField] private GameObject choiceIndicator;
	[SerializeField] private GameObject switchPageIndicator;
	[SerializeField] private GameObject choiceContainer;
	[SerializeField] private GameObject dialogueContainer;
	[SerializeField] private GameObject choicePrefab;
	[SerializeField] private RectTransform textBoxTransform;

	private PanelConfig rightPanel;
	private PanelConfig leftPanel;

	private DialogueManager dialogueManager;
	private DialogueBehavior dialogueBehavior;
	private Dialogue currentDialogue;

	private Coroutine dialogueCoroutine;

	private Color maskActiveColor = new Color(103.0f/255.0f, 101.0f/255.0f, 101.0f/255.0f);

	private GameObject[] choiceObjects; 

	private bool introPlayed = false;
	private bool reachedEnding = false;
	private bool waitingForChoice = false;
	public bool dialogueStarted = false;

	private float defaultTimePerLetter = 0.05f;
	private float punctuationDelay = 0.25f;
	private float timePassedSincePress = 0f;

	private const int MaxNumberOfChoicesPerPage = 5;
	private const float IndicatorXPadding = 0f;
	private const float ButtonDelay = 0.7f;
	private const float ChoiceButtonDelay = 0.2f;
	private const float HoldButtonDelay = 0.1f;

	private const float IndicatorVolume = 0.1f;

	private int selectedChoiceIndex = 0;

	private const string BulletColorStartTag = "<color=E8E8E8FF> ";
	private const string ColorEndTag = "</color> ";
	private const string MessageColorStartTag = "<color=E8E8E8FF> ";

	[SerializeField] private AudioClip confirmSound;
	[SerializeField] private Color dialogueColor = Color.white;

	// BootSequence -> Initialize Panels (activePanel.Configure ()) ->
	// -> UpdatePanelState upon spacebar input -> ConfigurePanels ->  
	public void BootSequence() {
		Debug.Log(string.Format ("{0} is booting up.", GetType().Name));
		dialogueManager = MasterManager.dialogueManager;
		dialogueBehavior = GameObject.Find("Player").GetComponent<DialogueBehavior>();
		rightPanel = GameObject.Find("RightCharacterPanel").GetComponent<PanelConfig>();
		leftPanel = GameObject.Find("LeftCharacterPanel").GetComponent<PanelConfig>();
		Debug.Log(string.Format ("{0} status = {1}", GetType().Name, currentState));
	}

	public void InitializeConversation (int conversationID) {
		LoadConversation(conversationID);
		InitializePanels();
		ConfigureTextBox();
		dialogueText.color = dialogueColor;
	}

	// in the beginning of setup, first the left and right panel are initialized
	private void InitializePanels() {
		leftPanel.Configure(currentDialogue);
		leftPanel.SetTextColor(dialogueColor);
		rightPanel.Configure(currentDialogue);
		rightPanel.SetTextColor(dialogueColor);

		StartCoroutine(MasterManager.animationManager.IntroAnimation());
	}

	void ConfigureTextBox () {
	    if (introPlayed) {
	    	PrintDialogueDebugInfo();
	    	dialogueCoroutine = StartCoroutine(AnimateText(currentDialogue.message));
	  		dialogueStarted = true;
	  		dialogueFinished = false;
	    } else {
	    	StartCoroutine(WaitForIntroAnimation());
	    }
	}

	void Update () {
		if (waitingForChoice) {
			UpdateChoicesSystem();
		} else {
			UpdateDialogueSystem();
		}
	}

	// if selectedChoiceIndex is at -1 that means its pointing to the
	// next page button. 
	// -1 = nextPageButton
	// 0 = 1st choice
	// 1 = 2nd choice
	// 2 = 3rd choice
	// 3 = 4th choice
	// so on...


	void UpdateChoicesSystem () {
		timePassedSincePress += Time.deltaTime;

		if (timePassedSincePress > ChoiceButtonDelay) {
			if (dialogueBehavior.upPressed && dialogueBehavior.GetButtonHoldTime(1) > 0) {
				selectedChoiceIndex++;
			} else if (dialogueBehavior.downPressed && dialogueBehavior.GetButtonHoldTime(2) > 0) {
				selectedChoiceIndex--;
			} else if (dialogueBehavior.confirmPressed && dialogueBehavior.GetButtonHoldTime(0) > 0) {
				ProcessConfirm();
				ExitChoiceSystem();
			}
			Debug.Log(dialogueBehavior.GetButtonHoldTime(1));
			timePassedSincePress = 0;

			// minus one to accomodate for zero indexing
			selectedChoiceIndex = (selectedChoiceIndex < 0) ? choiceObjects.Length - 1 : 
				(selectedChoiceIndex > choiceObjects.Length - 1) ? 0 : selectedChoiceIndex;

			DrawChoiceIndicator();
		}
	}

	private void ProcessConfirm () {
		dialogueManager.ProcessDialogueCommand(currentDialogue.choices[selectedChoiceIndex]);
	}

	private void DrawChoiceIndicator () {
		var xPadding = 15;
		var yPadding = 3;
		RectTransform indicatorRectTrans = choiceIndicator.GetComponent<RectTransform>();
		var objectSelected = choiceObjects[selectedChoiceIndex];
		var targetObjectRectTrans = objectSelected.GetComponent<RectTransform>();
		var newXPos = (targetObjectRectTrans.localPosition.x - targetObjectRectTrans.rect.width / 2f) - xPadding;
		var newYPos = (targetObjectRectTrans.localPosition.y + targetObjectRectTrans.rect.height / 2.5f) - yPadding;
		indicatorRectTrans.localPosition = new Vector2(newXPos, newYPos);
	}

	void UpdateDialogueSystem () {
		if (!introPlayed) {
			return;
		}

		if (dialogueBehavior.confirmPressed && timePassedSincePress > ButtonDelay) {
			timePassedSincePress = 0;

			if (!dialogueFinished) {
				FinishDialogue();
				return;
			}

			if (reachedEnding) {
				ExitDialogue();	
				return;
			}

			if (dialogueFinished) {
				Debug.Log(currentDialogue.hasChoice);
				waitingForChoice = currentDialogue.hasChoice;
				if (currentDialogue.hasChoice) {
					waitingForChoice = currentDialogue.hasChoice;
					InitializeChoices();
					return;
				}
			}
			
			ProgressDialogue();
			reachedEnding = dialogueManager.isEnd;
		}

		timePassedSincePress += Time.deltaTime;
	}

	private void ProgressDialogue () {
		ProgressDialogueManager();
		UpdateCurrentDialogue();
		ConfigurePanels();
		ConfigureTextBox();
		reachedEnding = dialogueManager.isEnd;
	}
	private void InitializeChoices () {
		var textBoxHeight = textBoxTransform.rect.height;
		var xPadding = 0;
		var yPadding = 35;
		var currentNumberOfChoices = currentDialogue.choices.Count;

		float heightOfCell = textBoxHeight / currentNumberOfChoices;
		float midpointDistanceWithinCell = heightOfCell / 2;
		float currentCellYPos = textBoxTransform.rect.yMin;

		choiceObjects = new GameObject[currentNumberOfChoices];

		for (int i = 0; i < currentNumberOfChoices; i++){
			choiceObjects[i] = Instantiate(choicePrefab, choiceContainer.GetComponent<Transform>());
			var originalPos = choiceObjects[i].transform.localPosition;
			var newY = currentCellYPos + midpointDistanceWithinCell;

			choiceObjects[i].transform.localPosition = new Vector3(originalPos.x + xPadding, newY + yPadding, originalPos.z);
			// for the rich text feature, it is necessary to have start and end tags for every attribute
			choiceObjects[i].GetComponent<Text>().text =  BulletColorStartTag + "\u2022 " + ColorEndTag + 
				MessageColorStartTag + currentDialogue.choices[i].message + ColorEndTag;
			currentCellYPos += heightOfCell;
		}

		ToggleContainers();
		DrawChoiceIndicator();
	}

	private void ConfigurePanels () { 
		rightPanel.Configure(currentDialogue);
		leftPanel.Configure(currentDialogue);
	 }

	private void LoadConversation (int conversationID) {
		dialogueManager.LoadDialogue(conversationID);
		currentDialogue = dialogueManager.currentDialogue;
	}


	IEnumerator AnimateText(string message) {
		dialogueText.text = "";

		foreach(char letter in message) {
			dialogueText.text += letter;
			dialogueFinished = dialogueText.text == message;
			finishIndicator.SetActive(dialogueFinished);
			if (letter == '.' || letter == '!' || letter == '?') {
				PlayDialogueSound(currentDialogue.name);

				yield return new WaitForSeconds(defaultTimePerLetter + punctuationDelay);
			} else {
				PlayDialogueSound(currentDialogue.name);

				yield return new WaitForSeconds(defaultTimePerLetter);
			}

			if (dialogueFinished) {
				StopDialogueSound();
			} 
		}
	}

	IEnumerator WaitForIntroAnimation () {
		yield return new WaitForSeconds(1.5f);
		introPlayed = true;
		ConfigureTextBox();
	}

	// waits for two seconds for the panels to leave the screen before the text is cleared
	private IEnumerator ResetForNextDialogue () {
		yield return new WaitForSeconds(2);
		leftPanel.Reset();
		rightPanel.Reset();
		dialogueStarted = false;
		dialogueFinished = false;
	}

	private void ExitDialogue () {
		finishIndicator.SetActive(false);
		StartCoroutine(MasterManager.animationManager.ExitAnimation());
		StartCoroutine(ResetForNextDialogue());
		var gameManager = GameObject.Find("ManagerController").GetComponent<GameManager>();
        gameManager.ExitDialogue();
	}

	private void FinishDialogue () {
		StopCoroutine(dialogueCoroutine);
		dialogueFinished = true;
		dialogueText.text = currentDialogue.message;
		finishIndicator.SetActive(true);
	}

	private void PlayDialogueSound (string speakerName) {
		var soundManager = MasterManager.soundManager;
		var atlasManager = MasterManager.atlasManager;
		soundManager.PlayDialogueSound(atlasManager.LoadDialogueSound(speakerName));
	}

	private void StopDialogueSound () {
		MasterManager.soundManager.StopDialogueSound();
	}

	private void PlayConfirmChoiceSound () {
		MasterManager.soundManager.PlayDialogueSound(confirmSound, IndicatorVolume);
	}

	private void PrintDialogueDebugInfo () {
		Debug.Log(string.Format("conversationID {0} and currentDialogueID {1}", dialogueManager.conversationID, currentDialogue.ID));
		Debug.Log(string.Format("jumpID {0} and message {1}", currentDialogue.jumpID, currentDialogue.message));
	}

	private void ProgressDialogueManager () {
		dialogueManager.AdvanceDialogue();
	}

	private void UpdateCurrentDialogue () {
		currentDialogue = dialogueManager.currentDialogue;
	}

	public void SetTextBoxMask (bool toggle) {
		if (toggle) {
			textBoxImage.color = Color.white;
			finishIndicator.GetComponent<Image>().color = Color.white;
		} else {
			textBoxImage.color = maskActiveColor;
			finishIndicator.GetComponent<Image>().color = maskActiveColor;
		}

		finishIndicator.GetComponent<Animator>().SetBool("Pause", !toggle);
	}

	private void ToggleContainers () {
		dialogueContainer.SetActive(!waitingForChoice);
		choiceContainer.SetActive(waitingForChoice);
	}

	private void ExitChoiceSystem () {
		waitingForChoice = false;
		ProgressDialogue();
		ToggleContainers();
	}
}
