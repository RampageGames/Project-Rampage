using UnityEngine;
using System.Collections;

public class DetectPickup : MonoBehaviour {

	private bool triggered;
	private float respawn = 10f;
	private GameObject childObject;

	// Use this for initialization
	void Start () {

		triggered = false;
		childObject = transform.GetChild (0).gameObject;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (respawn <= 0) {
			triggered = false;
			collider.enabled = true;
			childObject.SetActive(true);
			respawn = 5f;
		}
		if (triggered) {
			collider.enabled = false;
			respawn -= Time.deltaTime;
			childObject.SetActive(false);
		}

	}

	void OnTriggerEnter(Collider col){
		triggered = true;
	}
}
