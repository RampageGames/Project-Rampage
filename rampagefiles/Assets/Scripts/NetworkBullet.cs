using UnityEngine;
using System.Collections;

public class NetworkBullet : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;

	bool gotFirstUpdate = false;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {
			//Do nothing
		}
		else {
			transform.position = Vector3.Lerp (transform.position, realPosition, 0.2f);
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, 0.2f);
		}
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		
		if (stream.isWriting) {
			//This is OUR player. We need to send our actual position to the network
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
		}
		else {
			//This is someone elses player. We need to receive their position (as of a few milliseconds ago, and update our version of that player)
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();

			if (gotFirstUpdate == false) {
				transform.position = realPosition;
				transform.rotation = realRotation;
				gotFirstUpdate = true;
			}
		}
	}
}
