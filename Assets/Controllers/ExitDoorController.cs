using UnityEngine;
using System.Collections;

public class ExitDoorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") { // Down let wandering monsters trigger next level
			Debug.Log ("Player found exit.");
			Debug.Log ("Next step: Get the next level to load.");
		}
	}
}
