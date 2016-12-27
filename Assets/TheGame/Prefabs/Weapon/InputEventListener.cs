using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class InputEventListener : MonoBehaviour {

	public Animator animator;
	public string InputEventName;
	public string TriggerName;

	void FixedUpdate(){
		if (CrossPlatformInputManager.GetButtonDown (InputEventName)) {
			animator.SetTrigger (TriggerName);
		}
	}


}