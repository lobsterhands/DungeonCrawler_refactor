using UnityEngine;
using System.Collections;
using System;

public class ExitDoorController : MonoBehaviour {

	Action cbExitDoorReached;

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
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
