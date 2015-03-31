using UnityEngine;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	public GameObject standbyCamera;
	SpawnSpot[] spawnSpots;

	public bool offlineMode = false;

	bool connecting = false;

	List<string> chatMessages;
	int maxChatMessages = 5;

	public float respawnTimer = 0;

	// Use this for initialization
	void Start() {
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
		PhotonNetwork.player.name = PlayerPrefs.GetString ("Username", "Awesome Dude");
		chatMessages = new List<string>();
	}

	void OnDestroy() {
		PlayerPrefs.SetString ("Username", PhotonNetwork.player.name);
	}

	public void AddChatMessage(string m) {
		GetComponent<PhotonView>().RPC ("AddChatMessage_RPC", PhotonTargets.All, m);
	}

	[RPC]
	void AddChatMessage_RPC(string m) {
		while (chatMessages.Count >= maxChatMessages) {
			chatMessages.RemoveAt(0);
		}

		chatMessages.Add(m);
	}

	void Connect() {
		PhotonNetwork.ConnectUsingSettings("Rampage v001");
	}

	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

		if (PhotonNetwork.connected == false && connecting == false) {

			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Username: ");
			PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
			GUILayout.EndHorizontal();

			if (GUILayout.Button("Single Player")) {
				connecting = true;
				PhotonNetwork.offlineMode = true;
				OnJoinedLobby();
			}

			if (GUILayout.Button("MultiPlayer")) {
				connecting = true;
				Connect();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		if (PhotonNetwork.connected == true && connecting == false) {
			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();

			foreach (string msg in chatMessages) {
				GUILayout.Label(msg);
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}

	void OnJoinedLobby() {
		Debug.Log ("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed() {
		Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom (null);
	}

	void OnJoinedRoom() {
		Debug.Log ("OnJoinedRoom");

		SpawnMyPlayer();
	}

	void SpawnMyPlayer() {

		AddChatMessage("Spawning player: " + PhotonNetwork.player.name);
		if (spawnSpots == null) {
			Debug.LogError ("No spawn locations found");
			return;
		}
		SpawnSpot mySpawnSpot = spawnSpots[Random.Range (0, spawnSpots.Length)];

		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate("PlayerController2", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		standbyCamera.SetActive(false);
		((MonoBehaviour)myPlayerGO.GetComponent("PlayerMovement")).enabled = true;
		//((MonoBehaviour)myPlayerGO.GetComponent("FPShooting")).enabled = true; // Red bombs that Cliff made
		((MonoBehaviour)myPlayerGO.GetComponent("MouseLook")).enabled = true;
		//((MonoBehaviour)myPlayerGO.GetComponent("ShootRocketLauncher")).enabled = true; // Raycast from tutorial
		((MonoBehaviour)myPlayerGO.GetComponent("CursorHideShow")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("GrenadeThrow")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("ActivateWeapon")).enabled = true;


		((MonoBehaviour)myPlayerGO.GetComponentInChildren<WeaponSwitching>()).enabled=true; // enables switching weapons

		myPlayerGO.transform.FindChild("Main Camera").gameObject.SetActive(true);

	}



	void Update() {
		if (respawnTimer > 0) {
			respawnTimer -= Time.deltaTime;

			if(respawnTimer <= 0) {
				//Time to respawn the player!
				SpawnMyPlayer ();
			}

		}

	}
}
