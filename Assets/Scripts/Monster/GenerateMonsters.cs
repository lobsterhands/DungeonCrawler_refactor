using UnityEngine;
using System.Collections;

public class GenerateMonsters : MonoBehaviour {

	//The image of the monster
	public GameObject[] monsters;

	//how many monsters... thisNum x thisNum tile area gets 1 monster
	public float monsterDensity = 4;
	public float monsterDensityModifier = 0.97f; 

	//the maze that was created
	private WorldController worldController;
	private World theWorld;



	//This changes the scale to the set number.  can be modifed to change
	// off of a multiplier
	//monster.transform.localScale = new Vector3(1.5f,1.5f,1.5f);






	// Use this for initialization
	void Start () {
		//find the maze generation script and get the board size
		worldController = gameObject.GetComponent<WorldController>();
		theWorld = worldController.getWorld;

		startGeneration ();
	}//Start

	public void startGeneration(){
		theWorld = worldController.getWorld;

		//how many monsters and roughly where
		int numMons = Mathf.FloorToInt((theWorld.Width+1)/monsterDensity);//how many monsters accross
		//int monstersX = Mathf.FloorToInt((theWorld.Width+1)/monsterDensity);//how many monsters accross
		//int monstersY = Mathf.FloorToInt(theWorld.Width/monsterDensity);//how many monsters go up

		Debug.Log ("monstersX: " + numMons);
		Debug.Log ("monstersY: " + numMons);

		int[] xMax = new int[numMons];
		int[] xMin = new int[numMons];
		int[] yMax = new int[numMons];
		int[] yMin = new int[numMons];

		Debug.Log ("monstersX: " + numMons);
		Debug.Log ("xMax.length: " + xMax.Length);

		//Min and Max x value for each monster
		for (int i = 0; i < numMons; i++) {
			Debug.Log ("theWorld.Width: " + theWorld.Width.ToString());
			xMax [i] = (theWorld.Width / numMons * (i + 1)) - 2;
			xMin [i] = (theWorld.Width / numMons * i) + 2;
			Debug.Log ("xMax[i]: " + xMax[i]);
			Debug.Log ("xMin[i]: " + xMin[i]);
		} 

		//Min and Max y value for each monster
		for (int i = 0; i < numMons; i++) {
			yMax [i] = (theWorld.Height/ numMons * (i + 1)) - 2;
			yMin [i] = (theWorld.Height / numMons * i) + 2; 
		}

		// Instantiate monsters
		for (int i = 0; i < numMons; i++) {
			for (int j = 0; j < numMons; j++) {
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
	}//startGeneration()


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
