using UnityEngine;
using System.Collections;

public class ObjectPosition : MonoBehaviour {

	void OnGUI() {
		//Use Main Camera and get position current object, but point position is pivot point
		Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
		//And size box, for example 100x50
		GUI.Box(new Rect(screenPos.x, Screen.height - screenPos.y, 100, 50), "myBox");
	}
}
