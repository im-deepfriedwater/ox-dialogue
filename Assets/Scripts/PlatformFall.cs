using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : MonoBehaviour {

	public float fallDelay = 3f;
	private Rigidbody2D rigidBody2D;

	// Use this for initialization
	void Awake () {
		rigidBody2D = GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.CompareTag("Player")) {
			Invoke ("Fall", fallDelay);
		}
	}

	void Fall () {
		rigidBody2D.isKinematic = false;
	}	
}
