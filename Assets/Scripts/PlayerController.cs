using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	//float smooth = 0.05f; // used for camera lerp

	Slider[] UI_sliders;
	Text[] UI_texts;

	int playerOnLevel; // Current level player is on
	public GameObject HUD;
	public Slider Health_UI;
	//stats for player
	public float speed = 3.0f;
	public float max_speed = 6.0f; // max speed is really 5
	//health
	public float start_health = 5.0f;
	public float current_health;
	public Slider health_slider;
	public float max_health = 5.0f;
	//armor
	public float start_armor = 3.0f;
	public float current_armor;
	public Slider armor_slider;
	public float max_armor = 3.0f;
	// *new code invalving input scripts
	public Buttons[] input;
	private Rigidbody2D body2d;
	private InputState inputState;
	// *
	Animator animator;
	bool running;
	new public GameObject camera;
	new public GameObject health_bar;
	// Use this for initialization
	void Start () {
		Debug.Log ("Caller: " + this.gameObject);
		playerOnLevel = 1;
		//
		HUD = GameObject.FindGameObjectWithTag ("HUD");
		//Health_UI = HUD.gameObject.GetComponentInChildren<Slider> ();
		UI_sliders = HUD.gameObject.GetComponentsInChildren<Slider> (); // UI_sliders[0] = health; [1] = armor
		UI_texts = HUD.gameObject.GetComponentsInChildren<Text> (); // UI_texts[0] = Level;

		//
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		//health_slider = GameObject.FindGameObjectWithTag ("health_slider") as Slider;
		camera.transform.position = this.transform.position + new Vector3 (0f, 0f, -7.5f);
		camera.transform.parent = this.transform;

		animator = GetComponent<Animator> ();
		// *new code invalving input scripts
		body2d = GetComponent<Rigidbody2D> ();
		inputState = GetComponent<InputState> ();
		current_health = start_health-2;
		//health_slider.value = current_health;
		// Health_UI.value = current_health;
		current_armor = start_armor;
		armor_slider.value = current_armor;
		// *
	}

	float rotatePlayerX = 0F;

	void Update() {
		UI_sliders [0].value = current_health;
		UI_sliders [1].value = current_armor;
	}

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


		//animator.SetBool ("Running", running);
		if (Input.GetKeyDown ("space")) {
//			animator.SetTrigger ("Attack");
		}

		//Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
		//GetComponent<Rigidbody2D> ().velocity = movement * speed;

		bool right = false;
		bool left = false;
		bool up = false;
		bool down = false;

		// *new code invalving input scripts
		if (Input.GetAxis ("Horizontal") > 0) { // right
			right = true;
			inputState.GetButtonValue (Buttons.Right);
		}
		if (Input.GetAxis ("Horizontal") < 0) {
			left = true;
		}
		if (Input.GetAxis ("Vertical") > 0) {
			up = true;
		}
		if (Input.GetAxis ("Vertical") < 0) { // right
			down = true;
		}

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
			velY *= down ? -1 : 1;
		} else {
			velY = 0;
		}
		//Debug.Log (velX + " " + velY);
		body2d.velocity = new Vector2(velX, velY);
		// *

		gameObject.transform.rotation = Quaternion.Euler (0, 0, rotatePlayerX);
		camera.transform.rotation = Quaternion.Euler (0, 0, 0); // Keep child-object camera from rotating
	}
	void change_HP(float change)
	{
		current_health += change;
		Health_UI.value = current_health;
		//health_slider.value = current_health;
		if (current_health > max_health)
			current_health = 5.0f;
	}

	//collision checking
	void  OnTriggerEnter2D(Collider2D other)
	{
		switch(other.gameObject.tag)
		{
		case "Speed_Boost":
			other.gameObject.SetActive (false);
			speed += 1.0f;
			if (speed < max_speed)
				speed += 1.0f;
			break;
		case "Health":
			other.gameObject.SetActive (false);
			if (current_health == max_health)
				break;
			change_HP(1);
			//if (current_health <= max_health) {
				//current_health += 1.0f;
				//health_slider.value = current_health;
			//}	
			break;
		case "Armor":
			other.gameObject.SetActive (false);
			if (current_armor <= max_armor) {
				current_armor += 1.0f;
				armor_slider.value = current_armor;		
			}
			break;

		}
	}

	public void incrementLevel() {
		playerOnLevel++;
	}

	public int currentLevel() {
		return playerOnLevel;
	}
}