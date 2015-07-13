using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
			GUI.Box (new Rect (Screen.width*0.045f, Screen.height*0.4f, 140, 150), "" + gameObject.tag); //+ "'s Unit " + gameObject.GetComponent<theUnit> ().ID);
			if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion != null) {
				if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().hasUnit == false
					&& gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().canMoveTo == true) {
					if (GUI.Button (new Rect (Screen.width*0.05f, Screen.height*0.45f, 120, 20), "Move/End")) {
						if (gameObject.GetComponent<theUnit> ().MoveToSelectedTile () == true) {

							Reset ();
							
							Debug.Log ("Moved, End Turn");
						}
					}
				} else if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().hasUnit == true
					&& gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().region_Owner == gameObject.GetComponent<theUnit> ().ID
					&& gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().canMoveTo == true) {
					if (GUI.Button (new Rect (Screen.width*0.05f, Screen.height*0.45f, 120, 20), "Move/End")) {
						if (gameObject.GetComponent<theUnit> ().MoveToSelectedTile () == true) {
									
							Reset ();
									
							Debug.Log ("Moved, End Turn");
						}
					}
				} else if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().hasUnit == true
					&& gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().region_Owner != gameObject.GetComponent<theUnit> ().ID
					&& gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().canMoveTo == true) {
					if (GUI.Button (new Rect (Screen.width*0.05f, Screen.height*0.45f, 120, 20), "Attack/End")) {
						if (gameObject.GetComponent<theUnit> ().AttackSelectedTile () == true) {
						
							Reset ();
						
							Debug.Log ("Attacked, End Turn");
						}
					}
				}
			} else if(gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion == null) {
				if (GUI.Button (new Rect (Screen.width*0.05f, Screen.height*0.45f, 120, 20), "Train")) {
						gameObject.GetComponent<theUnit>().TrainUnit();
					}
			}
			// Lock and Unlock Selected Unit
			if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit == false) {
				if (GUI.Button (new Rect (Screen.width*0.05f, Screen.height*0.485f, 120, 20), "Lock Unit")) {
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit = true;
				}
			} else if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit == true) {
				if (GUI.Button (new Rect (Screen.width*0.05f, Screen.height*0.485f, 120, 20), "Unlock Unit")) {
					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit = false;
				}
			}

//			//Lock and Unlock Selected Tile
//			if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile == false) {
//				if (GUI.Button (new Rect (Screen.width*0.05f, Screen.height*0.52f, 120, 20), "Lock Tile")) {
//					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile = true;
//				}
//			} else if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile == true) {
//				if (GUI.Button (new Rect (Screen.width*0.05f, Screen.height*0.52f, 120, 20), "Unlock Tile")) {
//					gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile = false;
//				}
//			}
		}
	}

	void Reset(){
		// End Turn
		gameObject.GetComponent<theUnit> ().endTurn = true;
		
		// Reset REGION
		gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript>().isSelected = false;
		gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedRegion = null;
		gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().currentRegion = null;
		
		// Reset UNIT
		gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().previousUnit = gameObject.GetComponent<theUnit> ().theMap.GetComponent<Map> ().selectedUnit;
		gameObject.GetComponent<theUnit> ().beingControlled = false;
		gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().selectedUnit = null;
		
		// Turn-Based
		if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying == 1) {
			gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying = 2;
		} else if (gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying == 2) {
			gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().Turns -= 1;
			gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().whosPlaying = 1;
		}
		
		// Reset LOCKS
		gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedUnit = false;
		gameObject.GetComponent<theUnit> ().theMap.GetComponent<MapScript> ().lockSelectedMapTile = false;


	}
}
