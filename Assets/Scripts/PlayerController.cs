using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 3f;
	float smooth = 0.05f; // used for camera lerp

	//public GameObject gameSpaceObj;
	//public GameSpace gameSpaceScript;

	Animator animator;
	bool running;
	new public GameObject camera;

	// Use this for initialization
	void Start () {
	
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.transform.position = this.transform.position + new Vector3 (0f, 0f, -7.5f);
		camera.transform.parent = this.transform;

		animator = GetComponent<Animator> ();
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
			//rotatePlayerX = -90F;
		} else if (moveHorizontal < 0) {
			//rotatePlayerX = 90F;
		} else if (moveVertical > 0) {
			//rotatePlayerX = 0F;
		}  else if (moveVertical < 0) {
			//rotatePlayerX = 180F;
		}

		animator.SetBool ("Running", running);

		Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
		GetComponent<Rigidbody2D> ().velocity = movement * speed;

		gameObject.transform.rotation = Quaternion.Euler (0, 0, rotatePlayerX);
		camera.transform.rotation = Quaternion.Euler (0, 0, 0); // Keep child-object camera from rotating
	}
}
