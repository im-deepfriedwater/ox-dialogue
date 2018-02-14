using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	private InputState inputState;
	private Walk walkBehavior;
	private Animator animator;
	private CollisionState collisionState;
	private Duck duckBehavior;
	private DialogueBehavior dialogueBehavior;
	[SerializeField]
	public GameManager gameManager {get; private set;}

	void Awake () {
		inputState = GetComponent<InputState>();
		walkBehavior = GetComponent<Walk>();
		animator = GetComponent<Animator>();
		collisionState = GetComponent<CollisionState>();
		duckBehavior = GetComponent<Duck>();
		dialogueBehavior = GetComponent<DialogueBehavior>();
	}

	// Use this for initialization
	void Start () {
		// Time.timeScale = 0.001f;
	}
	
	// Update is called once per frame Controls Animation State
	void Update () {
		if (collisionState.standing) {
			ChangeAnimationState(0);
		}

		if (inputState.absVelX > 0) {
			ChangeAnimationState(1);
		}

		if (inputState.absVelY > 0) {
			ChangeAnimationState(2);
		}

		if (duckBehavior.ducking) {
			ChangeAnimationState(3);
		}

		if (!collisionState.standing && collisionState.onWall) {
			ChangeAnimationState(4);
		}

		animator.speed = walkBehavior.running ? walkBehavior.runMultiplier : 1;
	}

	private void ChangeAnimationState (int value) {
		animator.SetInteger ("AnimState", value);
	}

	public void InitializeDialogueControls (){
		dialogueBehavior.enabled = true;
		dialogueBehavior.EnterDialogue();
	}

	public void ExitDialogueControls (){
		dialogueBehavior.enabled = false;
		dialogueBehavior.ExitDialogue();
	}
	
}
