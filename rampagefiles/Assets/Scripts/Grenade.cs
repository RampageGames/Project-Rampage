using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour {

	//public variables
	public float radius = 5.0f;			//blast radius of the grenade
	public float explosiveTimer = 5.0f;	//explosion timer
	public float explosionDamage = 20f;	//damage of the grenade
	FXManager fxManager;
	//public Transform explosionFX;		//visual explosion effect
	//public AudioClip explosionAudio;	//audio explosion effect

	//private variables
	private Vector3 grenadeOrigin;		//position of the grenade
	private Collider[] collidersArray;	//array that holds colliders
	private bool exploded = false;
	private ArrayList alreadyHurtObjects;

	void Start () {
		alreadyHurtObjects = new ArrayList();
		fxManager = GameObject.FindObjectOfType<FXManager>();


		if (fxManager == null) {
			Debug.LogError("Couldn't find fxManager");
		}
	}

	/* 
	 * Waits until the explosion timer expires, and then proceeds
	 * to run the method(s) to hurt stuff
	 */ 
	void Update () {
		explosiveTimer -= Time.deltaTime;
		if (explosiveTimer <= 0 && exploded == false) {
			grenadeOrigin = this.transform.position; //Position of the grenade when it exploded
			FindAndHurtStuff(); 
			DestroyObject(gameObject); //Now destroy the grenade
			exploded = true;
		}

	}

	/* 
	 * This method finds all colliders in an explosion radius and checks to see that 
	 * there's line-of-sight between the explosion and the object. It then tries 
	 * to deal damage to that object
	 */ 
	void FindAndHurtStuff() {
		Vector3 target;
		collidersArray = Physics.OverlapSphere (grenadeOrigin, radius);	//All colliders hit by explosion

		foreach (Collider hit in collidersArray) { //For each collider that was inside the blast radius
			target = hit.transform.position;
			//Debug.DrawLine(grenadeOrigin, target,Color.red, 15f); //Used for debugging
			RaycastHit hitInfo;
			if (Physics.Linecast(grenadeOrigin, target, out hitInfo))
			{
				Debug.Log("linecast blocked between " + hit.name + " by " + hitInfo.collider.name);
				DealDamage(hitInfo.collider);
			}
			else {
				Debug.Log("clear path between " + hit.name);
				DealDamage(hit);
			}
		}
	}

	/* 
	 * This method takes a collider and tries to find a health component on that 
	 * object (or its parents), checks if it hasn't been hurt already by this grenade,
	 * then deals damage to it
	 */ 
	void DealDamage(Collider hit) {
			Health h = hit.transform.GetComponent<Health> (); //Get the health component
			if (h == null) { //If we didn't find a health component
				Transform hitTransform = hit.transform; //Get the transform of the collider
					
				while (h == null && hitTransform.parent) { //Loop through the parent objects looking for a health component. Exits when it finds one or when no more parents are left
					hitTransform = hitTransform.parent;
					h = hitTransform.transform.GetComponent<Health> ();			
				}
			}
			if (h != null) { //If it found a health component		
				PhotonView pv = h.GetComponent<PhotonView> (); //Get the PhotonView component
					
				if (pv == null) { //If there's no PhotonView component found, throw an error
					Debug.LogError ("No photon view found on target object");
			} 
			else {
				bool alreadyHurtBool = false;
				
				foreach(GameObject obj in alreadyHurtObjects) {
					if (obj == hit.gameObject) {
						alreadyHurtBool = true;
					}
				}
				
				if(alreadyHurtBool == false) {
					alreadyHurtObjects.Add (hit.gameObject);
					pv.RPC ("TakeDamage", PhotonTargets.AllBuffered, explosionDamage); //Deal the damage to the health component
				}
			}
		}
	}

	/* 
	 * This method takes a GameObject and destroys it either locally or over 
	 * the network, depending if its a networked GameObject
	 */ 
	void DestroyObject(GameObject gameObject){
		PhotonView pv = GetComponent<PhotonView> ();

		if (pv != null && pv.instantiationId != 0) {
			if (this.GetComponent<PhotonView>().isMine) {
				if (fxManager != null) {
					DoFX(grenadeOrigin);
				}
				PhotonNetwork.Destroy (gameObject);
			}
		} 
		else {
			if (fxManager != null) {
				DoFX(grenadeOrigin);
			}
			Destroy(gameObject);
		}
	}

	/*
	 * This method gets the position of the rocket
	 * calls the fxManager and does the special fx
	 */
	void DoFX(Vector3 grenadePos) {
		fxManager.GetComponent<PhotonView> ().RPC ("GrenadeFX", PhotonTargets.All, grenadePos);
	}
}
