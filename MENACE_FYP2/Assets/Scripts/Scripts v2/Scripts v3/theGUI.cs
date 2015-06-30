using UnityEngine;
using System.Collections;

public class theGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		// Make a background box
		if (gameObject.GetComponent<theUnit> ().beingControlled == true) {
			GUI.Box (new Rect (10, 10, 140, 150), "" + gameObject.tag); //+ "'s Unit " + gameObject.GetComponent<theUnit> ().ID);

			if (GUI.Button (new Rect (20, 40, 120, 20), "Move/Attack/End")) {
				if (gameObject.GetComponent<theUnit> ().MoveToSelectedTile () == true) {

					// End Turn
					gameObject.GetComponent<theUnit> ().endTurn = true;

					// Reset Selected Tile and Unit
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion = null;
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().previousUnit = gameObject.GetComponent<theUnit> ().theMap.GetComponent<Map> ().selectedUnit;
					gameObject.GetComponent<theUnit> ().beingControlled = false;
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedUnit = null;
						
					// Turn-Base
					if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying == 1) {
						gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying = 2;
					} else if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying == 2) {
						gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying = 1;
					}

					// Reset Locks
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit = false;
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile = false;
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().TURNSLEFT -= 1;
					Debug.Log ("Moved/Attacked, End Turn");
				}
				else{
					// End Turn
//					gameObject.GetComponent<theUnit> ().endTurn = true;
//					
//					// Reset Selected Tile and Unit
//					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion = null;
//					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().currentRegion = null;
//					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().previousUnit = gameObject.GetComponent<theUnit> ().theMap.GetComponent<Map> ().selectedUnit;
//					gameObject.GetComponent<theUnit> ().beingControlled = false;
//					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedUnit = null;
//					
//					// Turn-Base
//					if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying == 1) {
//						gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying = 2;
//					} else if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying == 2) {
//						gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying = 1;
//					}
//					
//					// Reset Locks
//					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit = false;
//					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile = false;
//					Debug.Log ("End Turn");
				}
			}

			// Lock and Unlock Selected Unit
			if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit == false) {
				if (GUI.Button (new Rect (20, 80, 120, 20), "Lock Unit")) {
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit = true;
				}
			} else if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit == true) {
				if (GUI.Button (new Rect (20, 80, 120, 20), "Unlock Unit")) {
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit = false;
				}
			}

			//Lock and Unlock Selected Tile
			if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile == false) {
				if (GUI.Button (new Rect (20, 120, 120, 20), "Lock Tile")) {
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile = true;
				}
			} else if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile == true) {
				if (GUI.Button (new Rect (20, 120, 120, 20), "Unlock Tile")) {
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile = false;
				}
			}

		} else {
			
		}
	}
}
