using UnityEngine;
using System.Collections;

public class GrenadeThrow : MonoBehaviour {

	//public variables
	//public Rigidbody grenade;	//inistialize grenade as a rigid body
	public GameObject grenade;
	public float fireRate = 1f;	//set the fire rate of the grenade
	public float throwPower = 10f;	//set the throwing power. higher is stronger
	//public float speed = 0f;

	//private variables
	private float counter = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.G) && counter >= fireRate)	//if the 'G' key is pressed and can attack
		{
			counter = 0;
			Camera cam = Camera.main;
			//Rigidbody clone = PhotonNetwork.Instantiate ("spacenade", transform.position, transform.rotation, 0)as Rigidbody;	//instantiates a clone of the grenade at its start point
			GameObject clone = (GameObject) PhotonNetwork.Instantiate("spacenade", cam.transform.position, cam.transform.rotation, 0);
			clone.GetComponent<Rigidbody>().detectCollisions = true;
			clone.transform.Translate(Vector3.forward * 1f);
			clone.rigidbody.AddForce(clone.transform.forward * throwPower, ForceMode.Impulse);
			//clone.transform.Translate(Vector3.forward * throwPower);
			//clone.velocity = transform.TransformDirection (Vector3.forward * throwPower);	//apply forward direction to the grenade multiplied by the throw power
			//clone.AddForce (transform.position * throwPower);	//CHECK IF USED
		}
		counter += Time.deltaTime;
	}
}
