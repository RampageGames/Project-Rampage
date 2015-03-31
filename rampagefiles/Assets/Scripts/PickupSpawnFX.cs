using UnityEngine;
using System.Collections;

public class PickupSpawnFX : MonoBehaviour {

	private Transform position;
	private Quaternion rotation;
	private float speed = 1;

	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.up * 1);
		if ( transform.position.y < 0f) {


		}
		if (transform.position.y > 2f ) {

		}
	
	}
}
