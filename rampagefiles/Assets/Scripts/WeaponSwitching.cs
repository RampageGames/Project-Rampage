using UnityEngine;
using System.Collections;

public class WeaponSwitching : MonoBehaviour {




	public GameObject[] weapons; // push your prefabs
	public Texture2D laserPistolTexture = null;
	public Texture2D laserPistolTextureOff = null;
	public Texture2D cowLauncherTexture = null;
	public Texture2D cowLauncherTextureOff = null;

	private GameObject currentPlayer;
	private WeaponSwitching ws;
	private PhotonView pv;
	private int currentWeapon = 0;
	private int maxWeapons = 5;	// 6 weapons, 0 is a weapon

	public bool rocketLauncherActive;
	public bool shotgunActive;


	void Start(){

		//((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = true;
		//((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
		currentPlayer = this.transform.gameObject;
		while (currentPlayer.transform.parent) {
			currentPlayer = currentPlayer.transform.parent.gameObject;
		}
		ws = currentPlayer.GetComponentInChildren<WeaponSwitching>();
		pv = ws.GetComponent<PhotonView>();

		pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 0);
		((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = true;
		((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
		((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
		((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;

		rocketLauncherActive = false;
		rocketLauncherActive = false;
	}

	void Update(){
		//pv.RPC ("SelectWeapon", PhotonTargets.AllBuffered, currentWeapon);
		/*Debug.Log ("current weapon " + currentWeapon);
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {

			if(currentWeapon + 1 <= maxWeapons){
				currentWeapon ++;
				//Debug.Log ("current weapon " + currentWeapon);
			}
			else
			{
				currentWeapon = 0;
				//Debug.Log ("current weapon " + currentWeapon);
			}
			//pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, currentWeapon);
		}
		else if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			
			if(currentWeapon - 1 >= 0){
				currentWeapon --;
				//Debug.Log ("current weapon " + currentWeapon);
			}
			else
			{
				currentWeapon = maxWeapons;
				//Debug.Log ("current weapon " + currentWeapon);
			}
			//pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, currentWeapon);
		}
//		if (currentWeapon == maxWeapons + 1) {
//			currentWeapon = 0;
//			Debug.Log ("current weapon " + currentWeapon);
//		}
//		if (currentWeapon == -1) {
//			currentWeapon = maxWeapons;
//			Debug.Log ("current weapon " + currentWeapon);
//		}*/

		if (Input.GetKeyDown ("1")) {
			//currentWeapon = 0;
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 0);

			//Debug.Log ("current weapon " + currentWeapon);
			//SelectWeapon (0);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = true;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;
		}
		if (Input.GetKeyDown ("2")) {
			//currentWeapon = 1;
			if(!rocketLauncherActive == false){
				pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 1);
				//Debug.Log ("current weapon " + currentWeapon);
				//SelectWeapon (1);
				((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
				((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = true;
				((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
				((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;
			}
		}
		if (Input.GetKeyDown ("3")) {
			//currentWeapon = 2;
			if(!shotgunActive == false){
				pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 2);

				//Debug.Log ("current weapon " + currentWeapon);
				//SelectWeapon (1);
				((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
				((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
				((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = true;
				((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;
			}
		}
		if (Input.GetKeyDown ("4")) {
			//currentWeapon = 3;
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 3);

			//Debug.Log ("current weapon " + currentWeapon);
			//			//SelectWeapon (1);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = true;
		}
		if (Input.GetKeyDown ("5")) {
			//currentWeapon = 4;
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 4);

			//Debug.Log ("current weapon " + currentWeapon);
			//			//SelectWeapon (1);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;
		}
		if (Input.GetKeyDown ("6")) {
			//currentWeapon = 5;
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 5);

			//Debug.Log ("current weapon " + currentWeapon);
						//SelectWeapon (1);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;
		}
		//WeaponChange ();
	}



	[RPC]
	void SelectWeapon(int index){
		foreach (GameObject obj in weapons) {
			obj.SetActive(false);
		}
		weapons[index].SetActive(true);
		currentWeapon = index;
		//Debug.Log ("current weapon " + currentWeapon);
	}

	public void ActivateWeapon(int num){
		if (num == 2) {
			rocketLauncherActive = true;
		}
		if (num == 3) {
			shotgunActive = true;
		}
	}

	void OnGUI() 
	{
		int currWeap = currentWeapon + 1;
		switch(currWeap){
		case 1:
			GUI.Label (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), laserPistolTexture);
			GUI.Label (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), cowLauncherTextureOff);
			GUI.Button (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), "Weapon3");
			GUI.Button (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "Weapon4");
			GUI.Button (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "Weapon5");
			GUI.Button (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "Weapon6");
			break;
		case 2:
			GUI.Label (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), laserPistolTextureOff);
			GUI.Label (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), cowLauncherTexture);
			GUI.Button (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), "Weapon3");
			GUI.Button (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "Weapon4");
			GUI.Button (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "Weapon5");
			GUI.Button (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "Weapon6");
			break;
		case 3:
			GUI.Button (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), "Weapon1");
			GUI.Button (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), "Weapon2");
			GUI.Button (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), "current");
			GUI.Button (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "Weapon4");
			GUI.Button (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "Weapon5");
			GUI.Button (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "Weapon6");
			break;
		case 4:
			GUI.Button (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), "Weapon1");
			GUI.Button (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), "Weapon2");
			GUI.Button (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), "Weapon3");
			GUI.Button (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "current");
			GUI.Button (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "Weapon5");
			GUI.Button (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "Weapon6");
			break;
		case 5:
			GUI.Button (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), "Weapon1");
			GUI.Button (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), "Weapon2");
			GUI.Button (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), "Weapon3");
			GUI.Button (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "Weapon4");
			GUI.Button (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "current");
			GUI.Button (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "Weapon6");
			break;
		case 6:
			GUI.Button (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), "Weapon1");
			GUI.Button (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), "Weapon2");
			GUI.Button (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), "Weapon3");
			GUI.Button (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "Weapon4");
			GUI.Button (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "Weapon5");
			GUI.Button (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "current");
			break;
		}
		
	}

	/*
	void WeaponChange(){

		//Debug.Log("wep chang");
		switch (currentWeapon) {
		case 0:
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 0);
			
		
			//SelectWeapon (0);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = true;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			break;
			
		case 1:
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 1);
			//Debug.Log ("current weapon " + currentWeapon);
			//SelectWeapon (1);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = true;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			break;
			
		case 2:
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 2);
			//Debug.Log ("current weapon " + currentWeapon);
			//SelectWeapon (1);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = true;
			break;
			
		case 3:
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 3);

			//SelectWeapon (1);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			break;
			
		case 4:
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 4);
			//Debug.Log ("current weapon " + currentWeapon);
			//SelectWeapon (1);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			break;
			
		case 5:
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 5);
		
			//SelectWeapon (1);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			break;
		}
	}*/
}