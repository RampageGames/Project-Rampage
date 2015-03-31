using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {
	
	public GameObject laserFXPrefab;	//laser fx
	public GameObject shotgunFXPrefab;	//shotgun fx
	public GameObject rocketFXPrefab;	//rocket explosion fx
	public GameObject grenadeFXPrefab;	//grenade explosion fx
	public GameObject beamFXPrefab;		//beam rifle fx

	/*
	 * 
	 */
	[RPC]
	void LaserFX(Vector3 startPos, Vector3 endPos) {
		Debug.Log ("laserFX");

		if(laserFXPrefab != null) {
			GameObject laserFX = (GameObject)Instantiate(laserFXPrefab, startPos, Quaternion.identity);
			LineRenderer lr = laserFX.transform.Find("LineFX").GetComponent<LineRenderer>();
			lr.SetPosition(0, startPos);
			lr.SetPosition(1, endPos);
		}
		else {
			Debug.LogError ("Missing laserFXPrefab");
		}

	}

	[RPC]
	void BeamFX(Vector3 startPos, Vector3 endPos) {
		Debug.Log ("beamFX");
		
		if(beamFXPrefab != null) {
			GameObject beamFX = (GameObject)Instantiate(beamFXPrefab, startPos, Quaternion.identity);
			LineRenderer lr = beamFX.transform.Find("LineFX").GetComponent<LineRenderer>();
			lr.renderer.material.mainTextureOffset = new Vector2 (0, Time.time);
			lr.SetPosition(0, startPos);
			lr.SetPosition(1, endPos);
		}
		else {
			Debug.LogError ("Missing laserFXPrefab");
		}
		
	}

	[RPC]
	void RocketFX(Vector3 explosionPos) {
		Debug.Log ("rocketFX");
		
		if(rocketFXPrefab != null) {
			GameObject rocketFX = (GameObject)Instantiate(rocketFXPrefab, explosionPos, Quaternion.identity);
		}
		else {
			Debug.LogError ("Missing rocketFXPrefab");
		}
		
	}

	[RPC]
	void GrenadeFX(Vector3 explosionPos) {
		Debug.Log ("grenadeFX");
		
		if(grenadeFXPrefab != null) {
			GameObject grenadeFX = (GameObject)Instantiate(grenadeFXPrefab, explosionPos, Quaternion.identity);
		}
		else {
			Debug.LogError ("Missing grenadeFXPrefab");
		}
		
	}
}
