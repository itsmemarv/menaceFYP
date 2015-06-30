using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public int tileX;
	public int tileY;

	public int tileOwner;
	public bool canMoveTo;

	public GameObject theMap;
	GameObject tempGO;
	public bool hasUnit;

	// Use this for initialization
	void Start () {
		theMap = GameObject.FindGameObjectWithTag ("MAP");
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTileColors ();

		if (canMoveTo == true) {
			gameObject.GetComponent<Renderer>().material.color = Color.yellow;
		}

//		if (theMap.GetComponent<Map> ().selectedUnit != null && theMap.GetComponent<Map> ().lockSelectedUnit == true) {
//
//			if (theMap.GetComponent<Map> ().lockSelectedMapTile == false) {
//				SelectTile ();
//			}
//		}
	}

	public void UpdateTileColors(){
		if(tileOwner == 0){
			gameObject.GetComponent<Renderer>().material.color = Color.white;
			gameObject.tag = "tile_Neutral";
		}
		else if(tileOwner == 1){
			gameObject.GetComponent<Renderer>().material.color = Color.cyan;
			gameObject.tag = "tile_Player1";
		}
		else if(tileOwner == 2){
			gameObject.GetComponent<Renderer>().material.color = Color.magenta;
			gameObject.tag = "tile_Player2";
		}
	}

//	void SelectTile() {
//		if (Input.GetMouseButtonDown (0)) {
//			Debug.Log ("Tile_Click!");
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hitInfo;
//
//			tempGO = theMap.GetComponent<Map> ().currentTile;
//
//			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {
//				if (hitInfo.collider.tag == "tile_Neutral") {
//					theMap.GetComponent<Map> ().selectedMapTile = hitInfo.transform.gameObject;
//					//Debug.Log ("X: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileX + " Y: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileY);
//					//Debug.Log ("OrigX: " + tempGO.GetComponent<Tile> ().tileX + " OrigY: " + tempGO.GetComponent<Tile> ().tileY);
//					
//				}
//			}
//		
//			Vector3 start = new Vector3 (tempGO.GetComponent<Tile> ().tileX, tempGO.GetComponent<Tile> ().tileY);
//			Vector3 end = new Vector3 (gameObject.GetComponent<Tile> ().tileX, gameObject.GetComponent<Tile> ().tileY);
//			//Debug.Log ("Start: " + start + " End: " + end);
//			Debug.DrawLine (start, end, Color.red, 2, false);
//		}
//	}


}
