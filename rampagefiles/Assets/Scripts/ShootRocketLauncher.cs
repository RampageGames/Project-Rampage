using UnityEngine;
using System.Collections;

public class ShootRocketLauncher : MonoBehaviour {

	public float cooldown = 0f;
	public GameObject rocket;
	public float firePower = 15f;
	public float fireRate = 1f;	//set the fire rate of the grenade
	public FirePoint firepoint;
	//public bool weaponActive;
	FXManager fxManager;
	//WeaponData weaponData;
	
	void Start() {
		fxManager = GameObject.FindObjectOfType<FXManager>();
		
		if (fxManager == null) {
			Debug.LogError("Couldn't find fxManager");
		}
	}
	
	// Update is called once per frame
	void Update () {

			cooldown -= Time.deltaTime;

		
			if (Input.GetButtonDown ("Fire1")) {
				//Player wants to shoot, so shoot
				Fire ();
			}

	}
	
	void Fire() {

		firepoint = gameObject.GetComponentInChildren<FirePoint>();
		if(cooldown > 0) {
			return;
		}
		
		Debug.Log("Firing our rocket");
		
		Camera cam = Camera.main;
		//Rigidbody clone = PhotonNetwork.Instantiate ("spacenade", transform.position, transform.rotation, 0)as Rigidbody;	//instantiates a clone of the grenade at its start point
		GameObject clone = (GameObject) PhotonNetwork.Instantiate("rocket", firepoint.position, cam.transform.rotation/*cam.transform.position, cam.transform.rotation*/, 0);
		clone.GetComponent<Rigidbody>().detectCollisions = true;
		clone.transform.Translate(Vector3.forward * 1f);
		clone.rigidbody.AddForce(clone.transform.forward * firePower, ForceMode.Impulse);
		
		cooldown = fireRate;
	}
	
}
