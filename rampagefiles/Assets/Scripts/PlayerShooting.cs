using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
	
	float cooldown = 0;
	FXManager fxManager;
	WeaponData weaponData;

	void Start() {
		fxManager = GameObject.FindObjectOfType<FXManager>();

		if (fxManager == null) {
			Debug.LogError("Couldn't find fxManager");
		}
	}
	
	// Update is called once per frame
	void Update () {
		cooldown -= Time.deltaTime;
	
		if (Input.GetButtonDown("Fire1")) {
			//Player wants to shoot, so shoot
			Fire();
		}
	}

	void Fire() {

		if(weaponData == null) {
			weaponData = gameObject.GetComponentInChildren<WeaponData>();
			
			if(weaponData == null) {
				Debug.LogError ("Did not find any weapon data in our children");
				return;
			}
		}

		if(cooldown > 0) {
			return;
		}

		Debug.Log("Firing our gun");

		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		Transform hitTransform;
		Vector3 hitPoint;

		hitTransform = FindClosestHitObject(ray, out hitPoint);

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
				//The line below is the network version of this line:
				//h.TakeDamage(damage);
				PhotonView pv = h.GetComponent<PhotonView>();

				if (pv == null) {
					Debug.LogError ("No photon view found on target object");
				}
				else {
				pv.RPC("TakeDamage", PhotonTargets.AllBuffered, weaponData.damage);
				}
			}

			if (fxManager != null) {
				DoGunFX(hitPoint);
			}

		}
		else {
			//We didn't hit anything (except empty space), but let's do a visual FX anyway
			if (fxManager != null) {
				hitPoint = Camera.main.transform.position + (Camera.main.transform.forward * 100f);
				DoGunFX(hitPoint);
			}
		}

		cooldown = weaponData.fireRate;
	}

	void DoGunFX(Vector3 hitPoint) {
		fxManager.GetComponent<PhotonView>().RPC ("LaserFX", PhotonTargets.All, weaponData.transform.position, hitPoint);
	}

	Transform FindClosestHitObject(Ray ray, out Vector3 hitPoint) {

		RaycastHit[] hits = Physics.RaycastAll(ray);

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
