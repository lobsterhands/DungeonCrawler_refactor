using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PickUp : MonoBehaviour {

	public GameObject[] pickUps;

	float[] probabilities = { 65f /*health*/, 30f /*armor*/, 5f /*speed boost*/ };

	public float pickupDensity = 1.4f; 

	private WorldController worldController;
	private World theWorld;

	// Use this for initialization
	void Start () {
		//find the maze generation script and get the board size
		worldController = gameObject.GetComponent<WorldController>();
		theWorld = worldController.getWorld;
	}

	public void generatePickups() {
		int numDeadEnds = worldController.deadEndLocations.Count;
		Debug.Log("Num deadEnds: " + numDeadEnds);

		int numPickups = Mathf.FloorToInt((numDeadEnds)/pickupDensity); // Calc number pickUps
		Debug.Log("Num pickups: " + numPickups);

		List<Vector2> randPos = chooseRandomPos (numPickups, numDeadEnds);
		for (int i = 0; i < numPickups; i++) {
			int element = chooseRandom (probabilities);
			Instantiate (pickUps [element], new Vector2 (randPos [i].x, randPos [i].y), Quaternion.identity);
		}
	}

	List<Vector2> chooseRandomPos(int nPickups, int nDeadEnds) {
		List<Vector2> result = new List<Vector2> ();

		int numToChoose = nPickups;

		for (int numLeft = nDeadEnds; numLeft > 0; numLeft--) {

			float prob = (float)numToChoose/(float)numLeft;

			if (Random.value <= prob) {
				numToChoose--;
				result.Add(worldController.deadEndLocations[numLeft - 1]);

				if (numToChoose == 0) {
					break;
				}
			}
		}

		return result;
	}

	int chooseRandom (float[] probs) {

		float total = 0;

		foreach (float elem in probs) {
			total += elem;
		}

		float randomPoint = Random.value * total;

		for (int i= 0; i < probs.Length; i++) {
			if (randomPoint < probs[i]) {
				return i;
			}
			else {
				randomPoint -= probs[i];
			}
		}
		return probs.Length - 1;
	}


}
