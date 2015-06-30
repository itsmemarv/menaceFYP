using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	// Tile Variables
	public GameObject Tile;
	private List<GameObject> tileList = new List<GameObject>();

	// Unit Variables
	public GameObject Unit;
	public List<GameObject> unitList = new List<GameObject>();

	// Map Variables
	int mapSizeX = 10;
	int mapSizeY = 10;

	public GameObject selectedUnit; 			// the unit that is being controlled
	public GameObject previousUnit;


	public GameObject currentTile; 				// the tile where the selected unit is on
	public GameObject selectedMapTile;
	public GameObject previousTile;

	
	public int whosPlaying;						// for turn-based
	public bool lockSelectedUnit; 				// to prevent selecting other unit
	public bool lockSelectedMapTile; 			// to prevent selecting other tiles

	// Use this for initialization
	void Start () {
		GenerateMap ();
		GenerateUnits ();
		GenerateUnitTags ();

		//selectedUnit = unitList [unitList.Count - 1]; // Initialise selected unit equals to the last in the list
		//previousUnit = unitList [unitList.Count - 1]; // Same goes here
		whosPlaying = 1;
		Debug.Log ("whosPlaying: " + whosPlaying);
		//CheckUnitOnTile ();
		lockSelectedUnit = false;
		lockSelectedMapTile = false;

		previousTile = null;

	}
	
	// Update is called once per frame
	void Update () {

		if (lockSelectedUnit == false) {
			SelectUnit ();
			//CheckUnitOnTile ();
			//RunTurnBased ();

		}
		if (selectedUnit != null && lockSelectedUnit == true) {
			if (lockSelectedMapTile == false) {
				SelectTile ();
			}
		}

		foreach (GameObject unit in unitList) {
			UpdateTileOwnership (unit);
		}
		//if (selectedUnit.GetComponent<theUnit> ().endTurn == true) {
		//RunTurnBased ();
		//CheckUnitOnTile ();
		//}

		CheckDistance();

		if (selectedUnit != null) {


			foreach (GameObject theTile in tileList) {
				if (selectedUnit.GetComponent<theUnit> ().posX == theTile.GetComponent<Tile> ().tileX &&
					selectedUnit.GetComponent<theUnit> ().posY == theTile.GetComponent<Tile> ().tileY) {
					currentTile = theTile;
					currentTile.GetComponent<Tile>().hasUnit = true;
				}
			}

		}
	}

	void GenerateMap(){
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeY; y++) {
				GameObject theTile = (GameObject)Instantiate( Tile, new Vector3(x, y, 0), Quaternion.identity );
				theTile.GetComponent<Tile>().tileX = x;
				theTile.GetComponent<Tile>().tileY = y;
				tileList.Add(theTile);
			}
		}
	}

	void GenerateUnits(){
		for (int i = 0; i < 2; ++i) {
			GameObject theUnit = (GameObject)Instantiate( Unit, new Vector3(Random.Range(0,10), Random.Range(0,10), -0.75f), Quaternion.identity );
			theUnit.GetComponent<theUnit>().posX = (int)theUnit.transform.position.x;
			theUnit.GetComponent<theUnit>().posY = (int)theUnit.transform.position.y;
			theUnit.GetComponent<theUnit>().ID = i+1;
			unitList.Add(theUnit);
		}
	}

	// Updates Tile Ownership
	void UpdateTileOwnership(GameObject goUnit){
		foreach (GameObject goTile in tileList) {
			if (goTile.tag == "tile_Neutral") {
				if (goTile.GetComponent<Tile> ().tileX == goUnit.GetComponent<theUnit> ().posX && goTile.GetComponent<Tile> ().tileY == goUnit.GetComponent<theUnit> ().posY) {
					goTile.GetComponent<Tile> ().tileOwner = goUnit.GetComponent<theUnit>().ID;
				}
			}
		}
	}
	
	// Divide the Units to two players
	void GenerateUnitTags(){
		float dividedListCount = unitList.Count * 0.5f;

		Debug.Log (dividedListCount);
		foreach (GameObject unit in unitList) {
			if(unit.GetComponent<theUnit>().ID <= dividedListCount)
			{
				unit.tag = "unit_Player1";
			}
			else if(unit.GetComponent<theUnit>().ID > dividedListCount)
			{
				unit.tag = "unit_Player2";
			}
		}
	}

//	void RunTurnBased(){
//		Debug.Log ("selUnit: " + (selectedUnit.GetComponent<theUnit> ().ID - 1));
//		Debug.Log ("I: " + i);
//		if (selectedUnit == unitList [i]) {
//			Debug.Log ("if");
//			selectedUnit.GetComponent<theUnit> ().beingControlled = false;
//			Debug.Log ("unitList.Count: " + unitList.Count);
//			if (i < unitList.Count-1) {
//				++i;
//				Debug.Log ("i++");
//			} else {
//				i = 0;
//				Debug.Log ("back2zero");
//			}
//			Debug.Log ("NEW I: " + i);
//			selectedUnit = unitList [i];
//			Debug.Log ("NEW selUnit: " + (selectedUnit.GetComponent<theUnit> ().ID - 1));
//			selectedUnit.GetComponent<theUnit> ().posX = (int)selectedUnit.transform.position.x;
//			selectedUnit.GetComponent<theUnit> ().posY = (int)selectedUnit.transform.position.y;
//			selectedUnit.GetComponent<theUnit> ().endTurn = false;
//			selectedUnit.GetComponent<theUnit> ().beingControlled = true;
//
//			Debug.Log ("I: " + i);
//				
//		}
//		CheckDistance();
//	}

	void CheckDistance(){
		foreach (GameObject myTile in tileList) {
			myTile.GetComponent<Tile>().canMoveTo = false;
			if(selectedUnit != null){
				int distance = (int)Vector3.Distance(
					new Vector3(selectedUnit.GetComponent<theUnit>().posX,selectedUnit.GetComponent<theUnit>().posY), 
					new Vector3(myTile.GetComponent<Tile>().tileX,myTile.GetComponent<Tile>().tileY));
				if (distance < 2 && distance > 0)
				{
					myTile.GetComponent<Tile>().canMoveTo = true;
				}
			}
		}
	}

	public void CheckUnitOnTile(){
		foreach (GameObject tile in tileList) {
			tile.GetComponent<Tile>().hasUnit = false;
			foreach(GameObject unit in unitList)
			{
				if(unit.GetComponent<theUnit>().posX == tile.GetComponent<Tile>().tileX &&
				   unit.GetComponent<theUnit>().posY == tile.GetComponent<Tile>().tileY)
				{
					tile.GetComponent<Tile>().hasUnit = true;
				}
			}
		}
	}

	void SelectUnit(){
		//Debug.Log ("Selecting a UNIT");
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Click!");
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {
				if (whosPlaying == 1) {
					if (hitInfo.collider.tag == "unit_Player1") {
						selectedUnit = hitInfo.transform.gameObject;
						selectedUnit.GetComponent<theUnit> ().beingControlled = true;
						//Debug.Log ("X: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileX + " Y: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileY);
						//Debug.Log ("OrigX: " + tempGO.GetComponent<Tile> ().tileX + " OrigY: " + tempGO.GetComponent<Tile> ().tileY);

					}
					else {
						selectedUnit = null;
					}
				} else if (whosPlaying == 2) {
					if (hitInfo.collider.tag == "unit_Player2") {
						selectedUnit = hitInfo.transform.gameObject;
						selectedUnit.GetComponent<theUnit> ().beingControlled = true;
						//Debug.Log ("X: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileX + " Y: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileY);
						//Debug.Log ("OrigX: " + tempGO.GetComponent<Tile> ().tileX + " OrigY: " + tempGO.GetComponent<Tile> ().tileY);

					}
					else {
						selectedUnit = null;
					}
				}
			} 
//			Vector3 start = new Vector3 (tempGO.GetComponent<Tile> ().tileX, tempGO.GetComponent<Tile> ().tileY);
//			Vector3 end = new Vector3 (gameObject.GetComponent<Tile> ().tileX, gameObject.GetComponent<Tile> ().tileY);
//			//Debug.Log ("Start: " + start + " End: " + end);
//			Debug.DrawLine (start, end, Color.red,2,false);

		}

	}

	void SelectTile(){
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Tile_Click!");
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;

			//GameObject tempTile = new GameObject();
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {
				if (hitInfo.collider.tag == "tile_Neutral") {
					selectedMapTile = hitInfo.transform.gameObject;
					if(selectedMapTile.GetComponent<Tile>().canMoveTo == true){
						//tempTile = selectedMapTile;
					} else {
						selectedMapTile = null;
						//tempTile = null;
					}
					//Debug.Log ("X: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileX + " Y: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileY);
					//Debug.Log ("OrigX: " + tempGO.GetComponent<Tile> ().tileX + " OrigY: " + tempGO.GetComponent<Tile> ().tileY);
				} 

			
				//Vector3 start = new Vector3 (currentTile.GetComponent<Tile> ().tileX, currentTile.GetComponent<Tile> ().tileY);
				//Vector3 end = new Vector3 (tempTile.GetComponent<Tile> ().tileX, tempTile.GetComponent<Tile> ().tileY);
				//Debug.Log ("Start: " + start + " End: " + end);
				//Debug.DrawLine (start, end, Color.red,2,false);
			
			}
		}
	
	}
}



























