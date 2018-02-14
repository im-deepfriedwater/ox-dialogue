using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEffect : MonoBehaviour {

	void OnDestroy () {
		Destroy (gameObject);
	}
}
