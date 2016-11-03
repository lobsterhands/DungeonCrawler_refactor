using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	//new public GameObject player = GameObject.FindGameObjectWithTag ("Player");
	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			animator.Play ("DungeonPlayerAttack", 0);
		}
	}
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Enemy" || collider.tag == "Player") {
			Destroy (collider.gameObject);
		}
	}
}