using UnityEngine;
using System.Collections;

public class WeaponSwitching : MonoBehaviour {




	public GameObject[] weapons; // push your prefabs
	public Texture2D laserPistolTexture = null;
	public Texture2D laserPistolTextureOff = null;
	public Texture2D cowLauncherTexture = null;
	public Texture2D cowLauncherTextureOff = null;
	public Texture2D shotgunTexture = null;
	public Texture2D shotgunTextureOff = null;
	public Texture2D weaponLocked = null;

	private Texture2D laserPistolIcon = null;
	private Texture2D rocketLauncherIcon = null;
	private Texture2D shotgunIcon = null;


	private GameObject currentPlayer;
	private WeaponSwitching ws;
	private PhotonView pv;
	private int currentWeapon = 0;
	private int maxWeapons = 5;	// 6 weapons, 0 is a weapon

	public bool laserPistolUnlocked;
	public bool laserPistolActive;
	public bool rocketLauncherUnlocked;
	public bool rocketLauncherActive;
	public bool shotgunUnlocked;
	public bool shotgunActive;


	void Start(){
		
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

		laserPistolUnlocked = true;
		laserPistolActive = true;
		rocketLauncherUnlocked = false;
		rocketLauncherActive = false;
		shotgunUnlocked = false;
		shotgunActive = false;
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
			IsActive(1);
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 0);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = true;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;
		}
		if (Input.GetKeyDown ("2")) {
			if(rocketLauncherUnlocked){
				IsActive(2);
				pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 1);
				((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
				((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = true;
				((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
				((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;
			}
		}
		if (Input.GetKeyDown ("3")) {
			if(shotgunUnlocked){
				IsActive(3);
				pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 2);
				((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
				((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
				((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = true;
				((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;
			}
		}
		if (Input.GetKeyDown ("4")) {
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 3);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = true;
		}
		if (Input.GetKeyDown ("5")) {
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 4);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;
		}
		if (Input.GetKeyDown ("6")) {
			pv.RPC("SelectWeapon", PhotonTargets.AllBuffered, 5);
			((MonoBehaviour)currentPlayer.GetComponent("ShootPistol")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootRocketLauncher")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootShotgun")).enabled = false;
			((MonoBehaviour)currentPlayer.GetComponent("ShootBeam")).enabled = false;
		}
		IconToDisplay ();
	}



	[RPC]
	void SelectWeapon(int index){
		foreach (GameObject obj in weapons) {
			obj.SetActive(false);
		}
		weapons[index].SetActive(true);
		currentWeapon = index;
	}

	public void ActivateWeapon(int num){
		if (num == 2) {
			rocketLauncherUnlocked = true;
		}
		if (num == 3) {
			shotgunUnlocked = true;
		}
	}

	void IsActive(int num)
	{
		if (num == 1) {
			laserPistolActive = true;
			rocketLauncherActive = false;
			shotgunActive = false;
		}
		if (num == 2) {
			laserPistolActive = false;
			rocketLauncherActive = true;
			shotgunActive = false;
		}
		if (num == 3) {
			laserPistolActive = false;
			rocketLauncherActive = false;
			shotgunActive = true;
		}

	}

	void IconToDisplay(){
		if (laserPistolActive) {
			laserPistolIcon = laserPistolTexture;
		}
		if (laserPistolUnlocked && !laserPistolActive) {
			laserPistolIcon = laserPistolTextureOff;
		}
		if (rocketLauncherActive){
			rocketLauncherIcon = cowLauncherTexture;
		}
		if (rocketLauncherUnlocked && !rocketLauncherActive) {
			rocketLauncherIcon = cowLauncherTextureOff;
		} 
		if (!rocketLauncherUnlocked) {
			rocketLauncherIcon = weaponLocked;
		}
		if (shotgunActive){
			shotgunIcon = shotgunTexture;
		}
		if (shotgunUnlocked && !shotgunActive) {
			shotgunIcon = shotgunTextureOff;
		} 
		if (!shotgunUnlocked) {
			shotgunIcon = weaponLocked;
		}
	}

	void OnGUI() 
	{
		int currWeap = currentWeapon + 1;
		switch(currWeap){
		case 1:
			GUI.Label (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), laserPistolIcon);
			GUI.Label (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), rocketLauncherIcon);
			GUI.Label (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), shotgunIcon);
			GUI.Label (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "Weapon4");
			GUI.Label (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "Weapon5");
			GUI.Label (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "Weapon6");
			break;
		case 2:
			GUI.Label (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), laserPistolIcon);
			GUI.Label (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), rocketLauncherIcon);
			GUI.Label (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), shotgunIcon);
			GUI.Label (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "Weapon4");
			GUI.Label (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "Weapon5");
			GUI.Label (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "Weapon6");
			break;
		case 3:
			GUI.Label (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), "Weapon1");
			GUI.Label (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), "Weapon2");
			GUI.Label (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), "current");
			GUI.Label (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "Weapon4");
			GUI.Label (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "Weapon5");
			GUI.Label (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "Weapon6");
			break;
		case 4:
			GUI.Label (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), "Weapon1");
			GUI.Label (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), "Weapon2");
			GUI.Label (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), "Weapon3");
			GUI.Label (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "current");
			GUI.Label (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "Weapon5");
			GUI.Label (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "Weapon6");
			break;
		case 5:
			GUI.Label (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), "Weapon1");
			GUI.Label (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), "Weapon2");
			GUI.Label (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), "Weapon3");
			GUI.Label (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "Weapon4");
			GUI.Label (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "current");
			GUI.Label (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "Weapon6");
			break;
		case 6:
			GUI.Label (new Rect ((Screen.width / 2) - 300, Screen.height - 100, 100, 100), "Weapon1");
			GUI.Label (new Rect ((Screen.width / 2) - 200, Screen.height - 100, 100, 100), "Weapon2");
			GUI.Label (new Rect ((Screen.width / 2) - 100, Screen.height - 100, 100, 100), "Weapon3");
			GUI.Label (new Rect (Screen.width / 2, Screen.height - 100, 100, 100), "Weapon4");
			GUI.Label (new Rect ((Screen.width / 2) + 100, Screen.height - 100, 100, 100), "Weapon5");
			GUI.Label (new Rect ((Screen.width / 2) + 200, Screen.height - 100, 100, 100), "current");
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