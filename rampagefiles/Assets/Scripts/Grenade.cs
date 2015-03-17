using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour {

	//public variables
	public float radius = 3.0f;	//blast radius of the grenade
	public float power = 350.0f;	//explosive power
	public float explosiveForce = 50.0f;	//explosive force
	public float explosiveTimer = 5.0f;		//explosion timer
	public float explosionDamage = 99f;	//damage of the grenade
	public Transform explosionFX;	//visual explosion effect
	//public AudioClip explosionAudio;	//audio explosion effect

	//private variables
	private Vector3 grenadeOrigin;	//position of the grenade
	private Collider[] colliders;	//array that holds colliders

	// Use this for initialization
	void Start () {
		//StartCoroutine(MyMethod());	//starts the coroutine
		//grenade ();
	}
	
	// Update is called once per frame
	void Update () {

		explosiveTimer -= Time.deltaTime;
		if (explosiveTimer <= 0) {
			MyMethod ();
		}

	}
	//public void MyMethod(){
	void MyMethod() {
		//yield return new WaitForSeconds (explosiveTimer);	//time to wait for before continuing
		grenadeOrigin = transform.position;
		//Instantiate (explosionFX, grenadeOrigin, transform.rotation);	//perform explosion at the grenades location
		colliders = Physics.OverlapSphere (grenadeOrigin, radius);	//

		foreach (Collider hit in colliders)
		{
			Debug.Log(hit);
			if(hit.rigidbody)
			{
				Health h = hit.transform.GetComponent<Health>();
				/*while (h == null && hit.parent) {
					hit = hit.parent;
					h = hit.transform.GetComponent<Health>();
				}*/
				if (h != null) {

					PhotonView pv = h.GetComponent<PhotonView>();
					
					if (pv == null) {
						Debug.LogError ("No photon view found on target object");
					}
					else {
						pv.RPC("TakeDamage", PhotonTargets.AllBuffered, explosionDamage);
					}
				}
				//Debug.Log(gameObject.name + "hit collider");
				//AudioSource.PlayClipAtPoint(explosionAudio, transform.position, 1);
				//hit.rigidbody.AddExplosionForce(power, grenadeOrigin, radius, explosiveForce);
				//hit.transform.SendMessage("applydamage", explosionDamage, SendMessageOptions.DontRequireReceiver);
			//this.rigidbody.AddExplosionForce(power, grenadeOrigin, radius, explosiveForce);	

				DestroyObject(gameObject);
			}
			else if(!hit.rigidbody)
			{
				Health h = hit.transform.GetComponent<Health>();

				if (h != null) {
					//The line below is the network version of this line:
					//h.TakeDamage(damage);
					PhotonView pv = h.GetComponent<PhotonView>();
					
					if (pv == null) {
						Debug.LogError ("No photon view found on target object");
					}
					else {
						pv.RPC("TakeDamage", PhotonTargets.AllBuffered, explosionDamage);
					}
				}
				//Debug.Log(gameObject.name + "hit collider");
				//hit.rigidbody.AddExplosionForce(power, grenadeOrigin, radius, explosiveForce);
				//AudioSource.PlayClipAtPoint(explosionAudio, transform.position, 1);
				//hit.transform.SendMessage("applydamage", explosionDamage, SendMessageOptions.DontRequireReceiver);
				//this.rigidbody.AddExplosionForce(power, grenadeOrigin, radius, explosiveForce);	
				DestroyObject(gameObject);
			}
		}
	}

	void DestroyObject(GameObject gameObject){
		PhotonView pv = GetComponent<PhotonView> ();
		Debug.Log (PhotonNetwork.isMasterClient);
		if (pv != null && pv.instantiationId != 0) {
		
			PhotonNetwork.Destroy (gameObject);
		} 
		else {
			Destroy(gameObject);
		}
	}

	/*void OnCollisionEnter(Collision col)
	{
		//Debug.Log(gameObject.name + "Collision detected with trigger object " + col.gameObject.name);
	}*/

	/*public void grenade(){
		StartCoroutine(MyMethod());

		grenadeOrigin = transform.position;
		colliders = Physics.OverlapSphere (grenadeOrigin, radius);
		
		foreach (Collider hit in colliders)
		{

			if(hit.rigidbody)
			{
				Debug.Log(gameObject.name + "hit collider");
				hit.rigidbody.AddExplosionForce(power, grenadeOrigin, radius, explosiveForce);
				//Destroy(gameObject);
			}
		}
		/*Collider[] hitColliders = Physics.OverlapSphere(grenadeOrigin, radius);
		int i = 0;
		while (i < hitColliders.Length) {
			//hitColliders[i].SendMessage("AddDamage");
			if(hit)
			Debug.Log(gameObject.name + "hit collider");
			Destroy(gameObject);
			i++;
		}*/
	//}



//	public void grenade(){
//		StartCoroutine(MyMethod());
//		grenadeOrigin = transform.position;
//		colliders = Physics.OverlapSphere (grenadeOrigin, radius);
//		
//		foreach (Collider hit in colliders)
//		{
//			if(hit.rigidbody)
//			{
//				hit.rigidbody.AddExplosionForce(power, grenadeOrigin, radius, explosiveForce);
//				//Destroy(gameObject);
//			}
//		}
//	}
}
