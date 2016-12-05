using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarController : MonoBehaviour {

	public Slider health_slider;
	public float start_health = 5.0f;
	public float current_health;
	public float max_health = 5.0f;
	// Use this for initialization
	void Awake () {
		health_slider = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		health_slider.value = current_health;
	}
}
