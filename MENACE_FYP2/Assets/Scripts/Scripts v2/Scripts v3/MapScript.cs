using UnityEngine;
using System.Collections.Generic;


//public enum turn_Choice{
//	MOVE = 0,
//	TRAIN = 1
//};


public class MapScript : MonoBehaviour {

	public static MapScript MapControl;
	public static MapScript RegionControl;

	public GameObject controller;

	public GameObject mapParent;
	// Region Variables
	public List<GameObject> regionList = new List<GameObject>();
	public GameObject regionParent;

	// Unit Variables
	public GameObject Unit;
	public List<GameObject> unitList = new List<GameObject>();

	public GameObject selectedUnit; 			// the unit that is being controlled
	public GameObject previousUnit;
	
	
	public GameObject currentRegion; 			// the region where the selected unit is on
	public GameObject selectedRegion;			// clicked region	
	public GameObject previousRegion;
	
	public int whosPlaying;						// for turn-based
	public bool lockSelectedUnit; 				// to prevent selecting other unit
	public bool lockSelectedMapTile; 			// to prevent selecting other tiles


	public bool StartOfGame;
	public bool QueueUnitTags;

	public int TURNSLEFT;
	public bool EndOfGame;
	public int player1counter;
	public int player2counter;
	public bool endCount;

//	public bool chooseMove;
//	public bool chooseTrain;

//	turn_Choice theChoice;


	void Awake(){
//		if (MapControl == null) {
//			DontDestroyOnLoad (mapParent);
//			MapControl = this;
//		} else if(MapControl != this){
//			Destroy(mapParent);
//		}
	}
	// Use this for initialization
	void Start () {
		StartOfGame = true;
		QueueUnitTags = false;
		whosPlaying = 1;
		lockSelectedUnit = false;
		lockSelectedMapTile = false;

		TURNSLEFT = 12;
		EndOfGame = false;
		player1counter = 0;
		player2counter = 0;
		endCount = false;


		controller = GameObject.Find ("SceneController");
		AddRegionInList ();
	}   
	
	// Update is called once per frame
	void Update () {

//		foreach (GameObject units in unitList) {
//			if (!controller.GetComponent<SceneController>().inBattleScene) {
//				units.SetActive (true);
//			} else 
//		if (controller.GetComponent<SceneController>().inBattleScene) {
//				units.SetActive (false);
//			}
//		}


		if (EndOfGame == false) {
			if (unitList.Count < 2) { 
				SelectRegion ();
			} else if (unitList.Count >= 2 && StartOfGame == true) {
				StartOfGame = false;
				QueueUnitTags = true;
			}

			if (QueueUnitTags == true) {
				GenerateUnitTags ();
				QueueUnitTags = false;
			}

			foreach (GameObject unit in unitList) {
				UpdateTileOwnership (unit);
			}
			if (lockSelectedUnit == false) {
				SelectUnit ();
			}

			if (selectedUnit != null && lockSelectedUnit == true) {
				if (lockSelectedMapTile == false) {
					SelectRegion ();
				}
			}
			if (selectedUnit != null) {
				foreach (GameObject theTile in regionList) {
					if (selectedUnit.GetComponent<theUnit> ().posX == theTile.GetComponent<RegionScript> ().regionX &&
						selectedUnit.GetComponent<theUnit> ().posY == theTile.GetComponent<RegionScript> ().regionY) {
						currentRegion = theTile;
						currentRegion.GetComponent<RegionScript> ().hasUnit = true;
					}
				}
			}

			checkNeighbors ();
	
			if (TURNSLEFT <= 0)
			{
				EndOfGame = true;
			}

		} else if (EndOfGame == true && endCount == false) {
			foreach(GameObject myUnits in unitList)
			{
				if(myUnits.GetComponent<theUnit>().tag == "unit_Player1"){
					player1counter++; 
				}
				else if(myUnits.GetComponent<theUnit>().tag == "unit_Player2"){
					player2counter++; 
				}
			}
			endCount = true;
		}
	}
	
	void SelectRegion(){
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray); // Cast a ray towards Z-axis in 2D Space(X-Y Axes)
			Debug.Log (hitInfo.transform.gameObject.name);
			foreach (GameObject theRegion in regionList)
			{
				if(hitInfo.transform.gameObject.name == theRegion.name)
				{
					selectedRegion = hitInfo.transform.gameObject;
					if(StartOfGame == true) // Instantiate first 2 units 
					{
						if(selectedRegion.GetComponent<RegionScript>().hasUnit == false){ // to prevent starting on the same regionee
							GameObject theUnit = (GameObject)Instantiate( Unit );
							//theUnit.transform.position = new Vector3(selectedRegion.transform.position.x,selectedRegion.transform.position.y, -0.1f);
							theUnit.transform.position = selectedRegion.transform.position;
							theUnit.GetComponent<theUnit>().posX = (float)theUnit.transform.position.x;
							theUnit.GetComponent<theUnit>().posY = (float)theUnit.transform.position.y;
							theUnit.GetComponent<theUnit>().ID = unitList.Count + 1;	// should start at 0;
							theUnit.transform.parent = controller.transform;
							controller.GetComponent<SceneController>().objectsInHierarchy.Add(theUnit);
							selectedRegion.GetComponent<RegionScript>().unitOnRegion = theUnit;
							unitList.Add (theUnit);
							selectedRegion.GetComponent<RegionScript>().hasUnit = true;
						}
					}
					else{
						selectedRegion.GetComponent<RegionScript> ().isSelected = true;
					}
				}
				else if (hitInfo.transform.gameObject.name != theRegion.name)
				{
					//Debug.Log ("not Selected");
					theRegion.GetComponent<RegionScript>().isSelected = false;
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
						if(selectedUnit != null){
							selectedUnit.GetComponent<theUnit> ().beingControlled = false; // old selected unit set control to false
							selectedUnit = hitInfo.transform.gameObject; 				   // the new selected Unit
							selectedUnit.GetComponent<theUnit> ().beingControlled = true;  // new selected unit set control to true
							
							//Debug.Log ("X: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileX + " Y: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileY);
							//Debug.Log ("OrigX: " + tempGO.GetComponent<Tile> ().tileX + " OrigY: " + tempGO.GetComponent<Tile> ().tileY);
						}
						else if(selectedUnit == null)
						{
							selectedUnit = hitInfo.transform.gameObject; 				   // the new selected Unit
							selectedUnit.GetComponent<theUnit> ().beingControlled = true;  // new selected unit set control to true
						}
					} else if (hitInfo.collider.tag != "unit_Player1"){
						selectedUnit.GetComponent<theUnit> ().beingControlled = false;
						selectedUnit = null;
					}
				} else if (whosPlaying == 2) {
					if (hitInfo.collider.tag == "unit_Player2") {
						if(selectedUnit != null){
							//selectedUnit.GetComponent<theUnit> ().beingControlled = false; // old selected unit set control to false
							selectedUnit = hitInfo.transform.gameObject; 				   // the new selected Unit
							selectedUnit.GetComponent<theUnit> ().beingControlled = true;  // new selected unit set control to true

						//Debug.Log ("X: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileX + " Y: " + theMap.GetComponent<Map> ().selectedMapTile.GetComponent<Tile> ().tileY);
						//Debug.Log ("OrigX: " + tempGO.GetComponent<Tile> ().tileX + " OrigY: " + tempGO.GetComponent<Tile> ().tileY);
						}
						else if(selectedUnit == null)
						{
							selectedUnit = hitInfo.transform.gameObject; 				   // the new selected Unit
							selectedUnit.GetComponent<theUnit> ().beingControlled = true;  // new selected unit set control to true
						}
					} else if (hitInfo.collider.tag != "unit_Player2"){
						selectedUnit.GetComponent<theUnit> ().beingControlled = false;
						selectedUnit = null;
					}
				}
			}
		}
	}

	void GenerateUnitTags(){
		float dividedListCount = unitList.Count * 0.5f;
		
		//Debug.Log (dividedListCount);
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

	void UpdateTileOwnership(GameObject goUnit){
		foreach (GameObject goTile in regionList) {
			if (goTile.GetComponent<RegionScript>().region_ID == 0) {
				if (goTile.GetComponent<RegionScript> ().regionX == goUnit.GetComponent<theUnit> ().posX && goTile.GetComponent<RegionScript> ().regionY == goUnit.GetComponent<theUnit> ().posY) {
					goTile.GetComponent<RegionScript> ().region_Owner = goUnit.GetComponent<theUnit>().ID;
				}
			}
		}
	}

	// THIS GONNA BE LONG!!!!!!
	void checkNeighbors(){
		if (currentRegion != null) {
			foreach(GameObject regionTile in regionList)
			{
				regionTile.GetComponent<RegionScript>().canMoveTo = false;
			}
			//Debug.Log ("Index of currentRegion: "+regionList.IndexOf (currentRegion));
			switch (regionList.IndexOf (currentRegion)) {
									// .. WEST CONTINENT .. //
			case 0: // west _1
				regionList [5].GetComponent<RegionScript> ().canMoveTo = true; // middle_2
				regionList [1].GetComponent<RegionScript> ().canMoveTo = true; // west_3
				regionList [2].GetComponent<RegionScript> ().canMoveTo = true; // west_2
				break;
			case 1: // west_2
				regionList [0].GetComponent<RegionScript> ().canMoveTo = true; // west_1
				regionList [2].GetComponent<RegionScript> ().canMoveTo = true; // west_3
				regionList [3].GetComponent<RegionScript> ().canMoveTo = true; // west_4
				break;
			case 2: // west_3
				regionList [0].GetComponent<RegionScript> ().canMoveTo = true; // west_1
				regionList [1].GetComponent<RegionScript> ().canMoveTo = true; // west_2
				regionList [3].GetComponent<RegionScript> ().canMoveTo = true; // west_4
				break;
			case 3: // west_4
				regionList [2].GetComponent<RegionScript> ().canMoveTo = true; // west_3
				regionList [1].GetComponent<RegionScript> ().canMoveTo = true; // west_2
				break;

								   // .. MIDDLE CONTINENT .. // 
			case 4: // middle_1
				regionList [5].GetComponent<RegionScript> ().canMoveTo = true; // middle_2
				regionList [6].GetComponent<RegionScript> ().canMoveTo = true; // middle_3
				break;
			case 5: // middle_2
				regionList [0].GetComponent<RegionScript> ().canMoveTo = true; // west_1
				regionList [4].GetComponent<RegionScript> ().canMoveTo = true; // middle_1
				regionList [6].GetComponent<RegionScript> ().canMoveTo = true; // middle_3
				regionList [7].GetComponent<RegionScript> ().canMoveTo = true; // middle_4
				break;
			case 6: // middle_3
				regionList [4].GetComponent<RegionScript> ().canMoveTo = true; // middle_1
				regionList [5].GetComponent<RegionScript> ().canMoveTo = true; // middle_2
				regionList [7].GetComponent<RegionScript> ().canMoveTo = true; // middle_4
				regionList [8].GetComponent<RegionScript> ().canMoveTo = true; // middle_5
				regionList [9].GetComponent<RegionScript> ().canMoveTo = true; // middle_6
				regionList [10].GetComponent<RegionScript> ().canMoveTo = true; // middle_7
				break;
			case 7: // middle_4
				regionList [5].GetComponent<RegionScript> ().canMoveTo = true; // middle_2
				regionList [9].GetComponent<RegionScript> ().canMoveTo = true; // middle_6
				regionList [6].GetComponent<RegionScript> ().canMoveTo = true; // middle_3
				break;
			case 8: // middle_5
				regionList [6].GetComponent<RegionScript> ().canMoveTo = true; // middle_3
				regionList [10].GetComponent<RegionScript> ().canMoveTo = true; // middle_7
				regionList [11].GetComponent<RegionScript> ().canMoveTo = true; // middle_8
				break;
			case 9: // middle_6
				regionList [7].GetComponent<RegionScript> ().canMoveTo = true; // middle_4
				regionList [6].GetComponent<RegionScript> ().canMoveTo = true; // middle_3
				regionList [10].GetComponent<RegionScript> ().canMoveTo = true; // middle_7
				regionList [15].GetComponent<RegionScript> ().canMoveTo = true; // east_4
				break;
			case 10: // middle_7
				regionList [9].GetComponent<RegionScript> ().canMoveTo = true; // middle_6
				regionList [6].GetComponent<RegionScript> ().canMoveTo = true; // middle_3
				regionList [8].GetComponent<RegionScript> ().canMoveTo = true; // middle_5
				regionList [11].GetComponent<RegionScript> ().canMoveTo = true; // middle_8
				break;
			case 11: // middle_8
				regionList [8].GetComponent<RegionScript> ().canMoveTo = true; // middle_5
				regionList [10].GetComponent<RegionScript> ().canMoveTo = true; // middle_7
				break;

									 // .. EAST CONTINENT .. //
			case 12: // east_1
				regionList [14].GetComponent<RegionScript> ().canMoveTo = true; // east_3
				regionList [13].GetComponent<RegionScript> ().canMoveTo = true; // east_2
				break;
			case 13: // east_2
				regionList [12].GetComponent<RegionScript> ().canMoveTo = true; // east_1
				regionList [14].GetComponent<RegionScript> ().canMoveTo = true; // east_3
				regionList [15].GetComponent<RegionScript> ().canMoveTo = true; // east_4
				break;
			case 14: // east_3
				regionList [12].GetComponent<RegionScript> ().canMoveTo = true; // east_1
				regionList [13].GetComponent<RegionScript> ().canMoveTo = true; // east_2
				regionList [15].GetComponent<RegionScript> ().canMoveTo = true; // east_4
				break;
			case 15: // east_4
				regionList [9].GetComponent<RegionScript> ().canMoveTo = true; // middle_6
				regionList [14].GetComponent<RegionScript> ().canMoveTo = true; // east_3
				regionList [13].GetComponent<RegionScript> ().canMoveTo = true; // east_2
				break;
			}
		}
	}

	void AddRegionInList(){
		foreach (Transform region in regionParent.transform) {
			regionList.Add(region.gameObject);
		}
	}

	void OnGUI(){
		if (unitList.Count < 2) {
			GUI.Box (new Rect (Screen.width * 0.40f, 0, 180, 25), "Players, Choose your Region!");
		} else if (EndOfGame == true){
			if(player1counter > player2counter){
			GUI.Box (new Rect (Screen.width * 0.45f, 0, 100, 25), "Player 1 WINS!");
			} else if(player1counter < player2counter){
				GUI.Box (new Rect (Screen.width * 0.45f, 0, 100, 25), "Player 2 WINS!");
			} else if(player1counter == player2counter){
				GUI.Box (new Rect (Screen.width * 0.45f, 0, 100, 25), "DRAW!!");
			}
		} else {
			GUI.Box (new Rect (Screen.width * 0.45f, 0, 100, 50), "Player " + whosPlaying + "'s Turn");
			GUI.Label (new Rect (Screen.width * 0.46f, 20, 100, 25), "Turns Left: " + TURNSLEFT);
		}

//		GUI.Box(new Rect(10,10, 180,110), "What do you want to do?");
//		if(chooseMove == false && (chooseTrain || !chooseTrain)){
//		chooseMove =  GUI.Toggle (new Rect (40, 40, 50, 20), chooseMove, "Move");
//
//				if(chooseTrain == true)
//					chooseTrain = false;
//		}
//
//		if(chooseTrain == false && (chooseTrain || chooseTrain)){
//		chooseTrain = GUI.Toggle (new Rect (110, 40, 50, 20), chooseTrain, "Train");
//			if(chooseMove == true)
//				chooseMove = false;
//		}
//
//		if(GUI.Button(new Rect(20,80,160,20), "Ok")) {
//			if(chooseMove && !chooseTrain)
//				theChoice = turn_Choice.MOVE;
//			else if(chooseTrain && !chooseMove)
//				theChoice = turn_Choice.TRAIN;
//		}
	}
}
