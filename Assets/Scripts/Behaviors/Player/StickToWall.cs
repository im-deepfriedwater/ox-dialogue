using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToWall : AbstractBehavior {

	public bool onWallDetected;

	protected float defaultGravityWallScale;
	protected float defaultDrag;


	// Use this for initialization
	void Start () {
		defaultGravityWallScale = body2d.gravityScale;
		defaultDrag = body2d.drag;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (collisionState.onWall) {
			if (!onWallDetected) {
				OnStick ();
				ToggleScripts(false);
				onWallDetected = true;
			}
		} else {
			if (onWallDetected) {
				OffWall ();
				ToggleScripts(true);
				onWallDetected = false;
			}
		}
	}

	protected virtual void OnStick () {
		if (collisionState.standing && body2d.velocity.y > 0) {
			body2d.gravityScale = 0;
			body2d.drag = 100;
		}
	}

	protected virtual void OffWall () {
		if (body2d.gravityScale != defaultGravityWallScale) {
	    	body2d.gravityScale = defaultGravityWallScale;
	    	body2d.drag = defaultDrag;
		}
	}


}
