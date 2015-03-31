using UnityEngine;
using System.Collections;

public class FirePoint : MonoBehaviour {

	public Vector3 position;
	public Quaternion rotation;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		position = this.transform.position;
		rotation = this.transform.rotation;
	}
}
