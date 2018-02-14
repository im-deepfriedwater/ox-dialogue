using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class AnimationManager : MonoBehaviour, IManager{

	// Use this for initialization
	public ManagerState currentState { get; private set; }

	[SerializeField] Animator panelAnimator;

	public void BootSequence () {
		Debug.Log(string.Format("{0} is booting up", GetType().Name));

		currentState = ManagerState.Completed;

        Debug.Log(string.Format("{0} status = {1}", GetType ().Name, currentState));

	}

	public IEnumerator IntroAnimation() {
		AnimationTuple introAnim = Constants.AnimationTuples.introAnimation;
		panelAnimator.SetBool(introAnim.parameter, introAnim.value);

		yield return new WaitForSeconds(1);
	}

	public IEnumerator ExitAnimation() {
		AnimationTuple exitAnim = Constants.AnimationTuples.exitAnimation;
		panelAnimator.SetBool(exitAnim.parameter, exitAnim.value);

		yield return new WaitForSeconds(1);
	}


}
