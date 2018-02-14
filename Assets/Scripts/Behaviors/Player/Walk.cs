using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : AbstractBehavior {

	public float speed = 50f;
	public float runMultiplier = 2f;
	public bool running;
	
	// Update is called once per frame
	void Update () {

		running = false;

		var right = inputState.GetButtonValue(inputButtons [0]);
		var left = inputState.GetButtonValue(inputButtons [1]);
		var run = inputState.GetButtonValue(inputButtons [2]);


		if (right || left) {
			var tmpSpeed = speed;

			if (run && runMultiplier > 0f) {
				tmpSpeed *= runMultiplier;
				running = true;
			}
			var velX = tmpSpeed * (float)inputState.direction;

			body2d.velocity = new Vector2(velX, body2d.velocity.y);
		}
	}
}
