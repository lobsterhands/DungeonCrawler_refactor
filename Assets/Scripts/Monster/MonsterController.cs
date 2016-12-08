using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

	public float moveSpeed;			//How fast should it move
	private bool moving;			//Are We moving
	private Vector3 moveDirection;	//What direction to move in
	private bool animationPlayed;	//Have we played the animation for the 

	public bool HaroldIsWatching;


	public float timeBetweenMove;			//how long to wait between stopping and starting
	public float timeToMove;				//how long to move
	private float timeBetweenMoveCounter;	//counting how long we have left before moving again
	private float timeToMoveCounter;		//counting how long we have left to move
	public float timeBetweenAttackCounter;	//Counting how long we have left before we can attack again

	private Rigidbody2D myRidgidbody;	//Contacting a player

	private GameObject thePlayer;			//Connect the player, so we can check its location/distance
	private PlayerController playerHealth;

	private Animation myAnimationController;	//the Animation Controller used to animate the model
	private Animation myAttakAnimController;	//Specificly for attack
	
	// Use this for initialization
	void Start () {
		timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
		timeToMoveCounter = Random.Range(timeToMove* 0.75f,timeToMove* 1.25f);

		myRidgidbody = this.GetComponent<Rigidbody2D>();

		myAnimationController = this.GetComponent<Animation> ();
		myAttakAnimController = this.GetComponent<Animation> ();
		myAttakAnimController.wrapMode = WrapMode.Once;

		thePlayer = GameObject.FindGameObjectWithTag ("Player");

		playerHealth = thePlayer.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (HaroldIsWatching == true) {
			Debug.Log ("Harold is watching.");
		}

		//if we have attacked recently, reduce the time till next attack.
		if (timeBetweenAttackCounter > 0.0f) {
			timeBetweenAttackCounter -= Time.deltaTime;

			if (timeBetweenAttackCounter < 0.0f) {
				timeBetweenAttackCounter = 0.0f;
			}
		}

		if (moving) {
			timeToMoveCounter -= Time.deltaTime;
			myRidgidbody.velocity = moveDirection;

			//Play the animation for moving
			//First, check to make sure we are not playing walk already.
			if(! myAnimationController.IsPlaying("run"))
			{
				playAnim ("run");
			}//if(myAnimationController.IsPlaying("walk"))

			if (timeToMoveCounter < 0f) {
				//Make the model stop moving
				moving = false;

				//make it stop moving
				myRidgidbody.velocity = Vector2.zero;

				//Generate the amount of time to be idle
				timeBetweenMoveCounter = Random.Range (timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);

				//Turn off the walk animation
				myAnimationController.Play("idle");
			}//if (timeToMoveCounter < 0f)
		}//if (moving)
		else{
			//Decrease the amount of time we have left to wait until we can move again
			timeBetweenMoveCounter -= Time.deltaTime;

			if (timeBetweenMoveCounter < 0f) {

				moving = true;
				timeToMoveCounter = Random.Range(timeToMove* 0.6f,timeToMove* 1.3f);

				if (canSeePlayer ()) 
				{
					moveDirection = thePlayer.transform.position - this.transform.position;
					timeToMoveCounter *= 5;
				} 
				else 
				{
					moveDirection = new Vector3 (Random.Range(-1f,1f)*moveSpeed,Random.Range(-1f,1f)*moveSpeed,0);
				}

				Vector3 lookdir = moveDirection;
				lookdir.Normalize ();
				float rotdir = Mathf.Atan2 (lookdir.z, lookdir.x) * Mathf.Rad2Deg;
				this.transform.rotation = Quaternion.Euler (0f, rotdir+45, 0f);


			}//if (timeBetweenMove < 0f)
		}//else //Not moving
			
	}//Update



	private void playAnim(string animationName)
	{
		myAnimationController.Play (animationName);
	}//void playAnim()


	private bool canSeePlayer()
	{
		Vector2 raycastDir = thePlayer.transform.position - this.transform.position;
		float maxSeeDistance = 7.0f;
		Vector2 startPos;
		startPos = this.GetComponent<Rigidbody2D> ().position;

		RaycastHit2D myHit = Physics2D.Raycast (startPos, raycastDir, maxSeeDistance);
		//Debug.DrawRay (this.transform.position, raycastDir, Color.red);
		//Debug.Log("hit: " + myHit.transform.name);

		if(myHit)
		{
			if (myHit.transform.tag == "Player") 
			{
				//we can see the player
				//float distance = Vector2.Distance(this.transform.position, thePlayer.transform.position);
				//Debug.Log("we can see the player");
				//Debug.Log("Distance: " + distance.ToString());

				return true;
			}
		}//if(Physics.Raycast(this.transform.position, raycastDir,RaycastHit))

		return false;
	}//private bool canSeePlayer()

	void OnCollisionEnter2D(Collision2D coll)
	{
		
		if (coll.transform.tag == "Player" && timeBetweenAttackCounter == 0.0f) 
		{
			myAttakAnimController.Play ("attack1");

			playerHealth.damagePlayer (1); 

			timeBetweenAttackCounter = Random.Range (0.75f, 1.25f);
		}
	}

}
