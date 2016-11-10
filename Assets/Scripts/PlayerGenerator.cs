using UnityEngine;
using System.Collections;

public class PlayerGenerator : MonoBehaviour {

	//the maze that was created
	//private WorldController worldController;
	//private World theWorld;
	public GameObject player;

	// Use this for initialization
	void Start () {
	
		//find the maze generation script and get the board size
	//	worldController = this.gameObject.GetComponent<WorldController>();
	//	theWorld = worldController.getWorld;

		//GameObject Player = 
		Instantiate (player, new Vector2 (1, 1), Quaternion.identity);// as GameObject;
		player.tag = "Player"; // Add tag to player for exit door trigger
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
