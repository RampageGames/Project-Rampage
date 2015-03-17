using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {
	
	public GameObject laserFXPrefab;

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
}
