using UnityEngine;
using System.Collections;

public class PlayerCrosshair : MonoBehaviour {

	public Texture2D chTexture;
	Rect positionch;
	static bool ch = true;

	// Update is called once per frame
	void Update () {
		positionch = new Rect((Screen.width - chTexture.width) / 2, (Screen.height - chTexture.height) /2, chTexture.width, chTexture.height);
	}

	void OnGUI() {
		if(ch == true)
		{
			GUI.DrawTexture(positionch, chTexture);
		}
	}
}