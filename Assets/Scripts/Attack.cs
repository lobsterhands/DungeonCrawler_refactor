using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Attack : MonoBehaviour {

	//new public GameObject player = GameObject.FindGameObjectWithTag ("Player");
	Animator animator;

	GameObject playerRHand;
	GameObject playerLHand;
	CircleCollider2D playerRHColl;
	CircleCollider2D playerLHColl;

	float attackTimer;
	bool isAttacking;
	public int monstersKilled = 0;

	//Text[] UI_texts;
	//public GameObject HUD;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		playerRHand = GameObject.FindGameObjectWithTag ("PlayerRHand");
		playerLHand = GameObject.FindGameObjectWithTag ("PlayerLHand");
		playerRHColl = playerRHand.GetComponent<CircleCollider2D> ();
		playerLHColl = playerLHand.GetComponent<CircleCollider2D> ();
		playerRHColl.enabled = false;
		playerLHColl.enabled = false;
		attackTimer = 0f;
		isAttacking = false;


		//UI_texts = HUD.gameObject.GetComponentsInChildren<Text> (); // UI_texts[0] = Level;, 1 = HP, 2 = Armor
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			attackTimer = 0f;
			isAttacking = true;
			animator.Play ("DungeonPlayerAttack", 0);
		}

		if (isAttacking) {
			playerRHColl.enabled = true;
			playerLHColl.enabled = true;
		} else {
			playerRHColl.enabled = false;
			playerLHColl.enabled = false;
		}

		//UI_texts [0].text = "Monster Kills: " + monstersKilled.ToString();
		//UI_texts [1].text = "Monster Kills: " + monstersKilled.ToString();
		//UI_texts [2].text = "Monster Kills: " + monstersKilled.ToString();
		//UI_texts [3].text = "Monster Kills: " + monstersKilled.ToString();

	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Enemy" || collider.tag == "Player") {
			Destroy (collider.gameObject);
			monstersKilled++;
		}
	}

	void FixedUpdate () {
		if (isAttacking) {
			attackTimer += Time.deltaTime;
			if (attackTimer > .4f) {
				isAttacking = false;
			}
		}
	}
}