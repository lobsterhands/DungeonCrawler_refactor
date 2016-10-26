using UnityEngine;
using System.Collections;

public class GenerateMonsters : MonoBehaviour {

	//The image of the monster
	public GameObject[] monsters;

	//how many monsters
	public int numMonsters = 16;

	//the maze that was created
	private WorldController worldController;
	private World theWorld;

	

	// Use this for initialization
	void Start () {
		//find the maze generation script and get the board size
		worldController = this.gameObject.GetComponent<WorldController>();
		theWorld = worldController.getWorld;

		//how many monsters and roughly where
		int monstersX = Mathf.FloorToInt(numMonsters/3);//how many monsters accross
		int monstersY = Mathf.FloorToInt(numMonsters/4);//how many monsters go up

		int[] xMax = new int[monstersX];
		int[] xMin = new int[monstersX];
		int[] yMax = new int[monstersY];
		int[] yMin = new int[monstersY];

		Debug.Log ("monstersX: " + monstersX);
		Debug.Log ("xMax.length: " + xMax.Length);

		//Min and Max x value for each monster
		for (int i = 0; i < monstersX; i++) {
			Debug.Log ("theWorld.Width: " + theWorld.Width);
			xMax [i] = theWorld.Width / monstersX * (i + 1);
			xMin [i] = (theWorld.Width / monstersX * i);
			Debug.Log ("xMax[i]: " + xMax[i]);
			Debug.Log ("xMin[i]: " + xMin[i]);
		}

		//Min and Max y value for each monster
		for (int i = 0; i < monstersY; i++) {
			yMax [i] = theWorld.Height/ monstersY * (i + 1);
			yMin [i] = (theWorld.Height / monstersY * i); 
		}

		// Instantiate monsters
		for (int i = 0; i < monstersX; i++) {
			for (int j = 0; j < monstersY; j++) {
				//Debug.Log ("i = " + i + ", j = " + j);
				//Debug.Log ("xMin = " + xMin[i] + ", xMax = " + xMax[i]);
				//Debug.Log ("yMin = " + yMin[j] + ", yMax = " + yMax[j]);

				//find the random point within the min and max for each dimension that we found earlier
				//then put it in a vector3 which will make the model apear above the floor.
				Vector2 monsterPos = new Vector2(genMonsterRand(xMin[i],xMax[i]),genMonsterRand(yMin[j],yMax[j]));
				Vector3 instPos = new Vector3 (monsterPos.x, monsterPos.y, -.15f);

				//find which monster we should use
				int whichMonster = Random.Range (0, monsters.Length);
				//Debug.Log (monsterPos);

				//instantiate the monster into the world.
				Instantiate (monsters [whichMonster], instPos, Quaternion.Euler (0, 180, 0));
			}
		}


	}//Start


	int genMonsterRand(int minPoint, int maxPoint){
		int randPoint;

		randPoint = Random.Range (minPoint, maxPoint);
		if (randPoint % 2 == 0) {
			int ran = Random.Range (0, 2);
			if (ran == 1) {randPoint++;} 
			else {randPoint--;}
		}//x is even, must be odd to be on a floor tile.

		return randPoint;
	}//genMonsterRand(2 ints)


}
