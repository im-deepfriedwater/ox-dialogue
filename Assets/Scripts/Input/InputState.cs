using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonState {
	public bool value;
	public float holdTime = 0f;  	
} 

public enum Directions {
	Right = 1,
	Left = -1
}

public class InputState : MonoBehaviour {

	public float absVelX = 0f;
	public float absVelY = 0f;
	public Directions direction = Directions.Right;

	private Rigidbody2D body2d; 
	private Dictionary<Buttons, ButtonState> buttonStates = new Dictionary<Buttons, ButtonState>();

	void FixedUpdate () {
		absVelX = Mathf.Abs(body2d.velocity.x);
		absVelY = Mathf.Abs(body2d.velocity.y);
	}

	public void Awake () {
		body2d = GetComponent<Rigidbody2D> ();
	}

	public void SetButtonValue (Buttons key, bool value) {
		if (!buttonStates.ContainsKey(key)) {
			buttonStates.Add (key, new ButtonState());
		}

		var state = buttonStates [key];

		if (state.value && !value) {
			state.holdTime = 0;
		} else if (state.value && value) {
			state.holdTime += Time.deltaTime;
		}

		state.value = value;
	}

	public bool GetButtonValue (Buttons key) {
		if (buttonStates.ContainsKey (key)) {
			return buttonStates[key].value;
		} else {
			return false;
		}
	}

	public float GetButtonHoldTime (Buttons key) {
		if (buttonStates.ContainsKey (key)) {
			return buttonStates[key].holdTime;
		} else {
			return 0;
		}
	}
}
