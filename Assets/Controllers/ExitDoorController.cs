using UnityEngine;
using System.Collections;
using System;

public class ExitDoorController : MonoBehaviour {
	Action cbExitDoorReached;

	GameObject player;
		
	void OnCollisionEnter2D(Collision2D collider) {
		if (collider.gameObject.tag == "Player") {

			player = GameObject.FindGameObjectWithTag ("Player");
			Debug.Log ("Increment player level");
			player.GetComponent<PlayerController> ().incrementLevel();
			Debug.Log("Level now: " + player.GetComponent<PlayerController> ().currentLevel ());

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
