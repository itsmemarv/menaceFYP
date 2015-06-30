using UnityEngine;
using System.Collections;

public class GUI_Script : MonoBehaviour {

	// Use this for initialization

	public void Start(){
	}

	public void Update(){
		if (gameObject.GetComponent<Unit> ().beingControlled == true) {
			Debug.Log ("GUIScript ID: " + gameObject.GetComponent<Unit> ().ID);
		} else {
		
		}
	}
	public void OnGUI(){
		// Make a background box
		if (gameObject.GetComponent<Unit> ().beingControlled == true) {
			GUI.Box (new Rect (10, 10, 100, 90), "" + gameObject.GetComponent<Unit> ().ID);
		
			if (GUI.Button (new Rect (20, 40, 80, 20), "Move IT")) {
				gameObject.GetComponent<Unit> ().MoveNextTile ();
			}
		} else {

		}
	}

}
