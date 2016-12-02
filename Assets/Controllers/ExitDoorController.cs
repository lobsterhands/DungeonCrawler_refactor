using UnityEngine;
using System.Collections;
using System;

public class ExitDoorController : MonoBehaviour {
	Action cbExitDoorReached;
			
	void OnCollisionEnter2D(Collision2D collider) {
		Debug.Log (collider.gameObject.tag);
		if (collider.gameObject.tag == "Player") {
			if (cbExitDoorReached != null) {
				cbExitDoorReached ();
			} else {
				Debug.LogError ("cbExitDoorReached is null.");
			}
		}
	}

	public void RegisterExitDoorReached(Action callback) {
		cbExitDoorReached += callback;
	}

	public void UnRegisterExitDoorReached(Action callback) {
		cbExitDoorReached -= callback;
	}
}
