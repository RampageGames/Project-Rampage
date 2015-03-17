using UnityEngine;
using System.Collections;

public class PlayerRaiseArm : MonoBehaviour {

	public GameObject target;

	void LateUpdate () {
		this.transform.rotation = target.transform.rotation;
	}
}
