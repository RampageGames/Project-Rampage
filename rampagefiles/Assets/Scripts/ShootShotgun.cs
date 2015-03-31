using UnityEngine;
using System.Collections;

public class ShootShotgun : MonoBehaviour {

	float cooldown = 0;
	FXManager fxManager;
	Transform hitTransform;
	Vector3 hitPoint;
	//WeaponData weaponData;
	//Vector3 firePoint;
	//GameObject firePoint;
	FirePoint firepoint;
	public float fireRate = 0.5f;
	public float damage = 9f;
	private int currentAmmo = 10;
	public float scaleLimit = 2.0f;    
	public float z = 5f;
	
	//Shoots multiple rays to check the programming
	public int count = 20;
	
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
			if(cooldown > 0) {
				return;
			}
			else{
				for( int i = 0; i < count; ++i ) {
					Fire ();  
				}
				cooldown = fireRate;
			}


		}
		
	}
	
	void Fire() {
		firepoint = gameObject.GetComponentInChildren<FirePoint>();

//		if(cooldown > 0) {
//			return;
//		}
		//  Try this one first, before using the second one
		//  The Ray-hits will form a ring
		float randomRadius = scaleLimit;             
		//  The Ray-hits will be in a circular area
		 //randomRadius = Random.Range( 0, scaleLimit );        
		
		float randomAngle = Random.Range (0, 2 * Mathf.PI );
		
//		//Calculating the raycast direction
//		Vector3 direction = new Vector3(
//			randomRadius * Mathf.Cos( randomAngle ),
//			randomRadius * Mathf.Sin( randomAngle ),
//			z
//			);
		Vector3 direction = Random.insideUnitCircle * scaleLimit;
		direction.z = z; // circle is at Z units 

		//Make the direction match the transform
		//It is like converting the Vector3.forward to transform.forward
		direction = Camera.main.transform.TransformDirection(direction.normalized);
		
		Debug.Log("Firing our gun");
		
		currentAmmo -= 1;
		
		Ray ray = new Ray(Camera.main.transform.position, direction);
//		Transform hitTransform;
//		Vector3 hitPoint;
		RaycastHit hit;

		//if (Physics.Raycast (ray, out hit)) {
			hitTransform = FindClosestHitObject (ray, out hitPoint);
		//}
		//else {
		//	Debug.Log("we hit nothing...");
		//}

		Debug.DrawLine( firepoint.position, hitPoint, Color.red, 15.0f); 
//		Debug.Log ("we hit " + hitTransform.transform.name);

		if(hitTransform != null) {
			
			Debug.Log ("We hit: " + hitTransform.transform.name);
			
			//We could do a special effect at the hit location
			//DoRicochetEffectAt(hitPoint); 
			
			Health h = hitTransform.transform.GetComponent<Health>();
			
			while (h == null && hitTransform.parent) {
				hitTransform = hitTransform.parent;
				h = hitTransform.transform.GetComponent<Health>();
			}
			
			//Once we reach here, hitTransform may not be the hitTransform we started with
			
			if (h != null) {

				PhotonView pv = h.GetComponent<PhotonView>();
				
				if (pv == null) {
					Debug.LogError ("No photon view found on target object");
				}
				else {
					pv.RPC("TakeDamage", PhotonTargets.AllBuffered, damage);
				}
			}
			
			if (fxManager != null) {
				DoGunFX(hitPoint);
			}
			
		}
		else {
			//We didn't hit anything (except empty space), but let's do a visual FX anyway
			if (fxManager != null) {
				hitPoint = Camera.main.transform.position + (Camera.main.transform.forward * 1f);
				DoGunFX(hitPoint);
			}
		}
		
//		cooldown = fireRate;
	}
	
	void DoGunFX(Vector3 hitPoint) {
			fxManager.GetComponent<PhotonView>().RPC ("LaserFX", PhotonTargets.All, firepoint.position, hitPoint);
	}
//	
//	void OnGUI() {
//		if(GetComponent<PhotonView>().isMine && gameObject.tag == "Player" ) {
//			if(GUI.Button (new Rect(Screen.width-100, Screen.height-40, 100, 40), currentAmmo.ToString())) {
//				
//				//Die ();
//			}
//		}
//	}
	
	Transform FindClosestHitObject(Ray ray, out Vector3 hitPoint) {
		
		RaycastHit[] hits = Physics.RaycastAll(ray, 20f);
		
		Transform closestHit = null;
		float distance = 0;
		hitPoint = Vector3.zero;
		
		foreach (RaycastHit hit in hits) {
			if (hit.transform != this.transform && (closestHit == null || hit.distance < distance)) {
				//We have hit something that is:
				// a) not us
				// b) the first thing we have hit that is not us
				// c) or, if not b, is at least closer than the previous closest thing
				
				closestHit = hit.transform;
				distance = hit.distance;
				hitPoint = hit.point;
			}
		}
		
		//closestHit is now either still null (we hit nothing) OR it contains the closest thing that is a valid thing to hit
		
		return closestHit;
	}
}