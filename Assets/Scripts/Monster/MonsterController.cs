using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

	public float moveSpeed;			//How fast should it move
	private bool moving;			//Are We moving
	private Vector3 moveDirection;	//What direction to move in


	public float timeBetweenMove;			//how long to wait between stopping and starting
	public float timeToMove;				//how long to move
	private float timeBetweenMoveCounter;	//counting how long we have left before moving again
	private float timeToMoveCounter;		//counting how long we have left to move

	private Rigidbody2D myRidgidbody;	//Contacting a player

	// Use this for initialization
	void Start () {
		timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
		timeToMoveCounter = Random.Range(timeToMove* 0.75f,timeToMove* 1.25f);

		myRidgidbody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (moving) {
			timeToMoveCounter -= Time.deltaTime;
			myRidgidbody.velocity = moveDirection;

			if (timeToMoveCounter < 0f) {
				moving = false;
				timeBetweenMoveCounter = Random.Range (timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
			}//if (timeToMoveCounter < 0f)
		}//if (moving)
		else{
			//make it stop moving
				myRidgidbody.velocity = Vector2.zero;

				timeBetweenMoveCounter -= Time.deltaTime;

				if (timeBetweenMoveCounter < 0f) {
					moving = true;
					timeToMoveCounter = Random.Range(timeToMove* 0.75f,timeToMove* 1.25f);

					moveDirection = new Vector3 (Random.Range(-1f,1f)*moveSpeed,Random.Range(-1f,1f)*moveSpeed,0);
				}//if (timeBetweenMove < 0f)
		}//else //Not moving


	}//Update
}
