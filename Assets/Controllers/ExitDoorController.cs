using UnityEngine;
using System.Collections;

public class ExitDoorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		Debug.Log ("Exit door clicked.");
		Debug.Log ("Next step: Get the next level to load.");
	}
}
