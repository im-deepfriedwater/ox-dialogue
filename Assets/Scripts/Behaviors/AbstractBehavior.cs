using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBehavior : MonoBehaviour {
	
	protected CollisionState collisionState;
	protected Rigidbody2D body2d;
	public Buttons[] inputButtons;
	public MonoBehaviour[] disableScripts;

	protected InputState inputState;

	protected virtual void Awake () {
		inputState = GetComponent<InputState> ();
		body2d = GetComponent<Rigidbody2D> ();
		collisionState = GetComponent<CollisionState> ();
	}

	
	protected void ToggleScripts (bool value) {
		foreach (var script in disableScripts) {
			script.enabled = value;
		}
	}
}
