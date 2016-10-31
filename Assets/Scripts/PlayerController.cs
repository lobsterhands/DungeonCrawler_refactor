using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public GameObject 

	public float speed = 3f;
	//float smooth = 0.05f; // used for camera lerp

	// *new code invalving input scripts
	public Buttons[] input;
	private Rigidbody2D body2d;
	private InputState InputState;
	// *

	Animator animator;
	bool running;
	new public GameObject camera;

	// Use this for initialization
	void Start () {
	
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.transform.position = this.transform.position + new Vector3 (0f, 0f, -7.5f);
		camera.transform.parent = this.transform;

		animator = GetComponent<Animator> ();

		// *new code invalving input scripts
		body2d = GetComponent<Rigidbody2D> ();
		inputState = GetComponent<InputState> ();
		// *
	}

	float rotatePlayerX = 0F;
	void FixedUpdate () {
	
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		if (moveHorizontal != 0 || moveVertical != 0) { // If the player is moving
			running = true;
		} else {
			running = false;
		}

		if (moveHorizontal > 0) {
			rotatePlayerX = -90F;
		} else if (moveHorizontal < 0) {
			rotatePlayerX = 90F;
		} else if (moveVertical > 0) {
			rotatePlayerX = 0F;
		}  else if (moveVertical < 0) {
			rotatePlayerX = 180F;
		}

		animator.SetBool ("Running", running);
		if (Input.GetKeyDown ("space")) {
			animator.SetTrigger ("Attack");
		}

		//Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
		//GetComponent<Rigidbody2D> ().velocity = movement * speed;

		// *new code invalving input scripts
		var up = inputState.GetButtonValue(input [0]);
		var down = inputState.GetButtonValue(input [1]);
		var left = inputState.GetButtonValue(input [2]);
		var right = inputState.GetButtonValue(input [3]);
		var velX = speed;
		var velY = speed;

		//Debug.Log (right + " " + left);
		//Debug.Log (up + " " + down);
		if (right || left) {
			velX *= left ? -1 : 1;
		} else {
			velX = 0;
		}
		if (up || down) {
			velY *= up ? -1 : 1;
		} else {
			velY = 0;
		}
		//Debug.Log (velX + " " + velY);
		GetComponent<Rigidbody2D> ().velocity = new Vector2(velX, velY);
		// *


		gameObject.transform.rotation = Quaternion.Euler (0, 0, rotatePlayerX);
		camera.transform.rotation = Quaternion.Euler (0, 0, 0); // Keep child-object camera from rotating
	}
}
