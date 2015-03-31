using UnityEngine;
using System.Collections;

public class ActivateWeapon : MonoBehaviour {

	private GameObject currentPlayer;
	private WeaponSwitching ws;
	private PhotonView pv;
	// Use this for initialization
	void Start () {
		currentPlayer = this.transform.gameObject;
		while (currentPlayer.transform.parent) {
			currentPlayer = currentPlayer.transform.parent.gameObject;
		}
		ws = currentPlayer.GetComponentInChildren<WeaponSwitching>();
		pv = ws.GetComponent<PhotonView>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		int weapon;
		Debug.Log ("we are in the collider");
		if (col.name == ("Rocket Spawn")) {
			weapon = 2;

			ws.ActivateWeapon(weapon);
		}
		Debug.Log ("we are in the collider");
		if (col.name == ("Shotgun Spawn")) {
			weapon = 3;
			
			ws.ActivateWeapon(weapon);
		}
	}
}
