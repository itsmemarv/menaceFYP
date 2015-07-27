using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

enum E_PHASE{
	ACQUIRE,
	DEPLOY,
	REINFORCE,
	ATTACK,
	FORTIFY,
	TOTAL
}

public class MapScript : MonoBehaviour {
	
	public static MapScript MapControl;
	public static MapScript RegionControl;
	
	public GameObject m_sceneController;
	public GameObject mapParent;

	// Player Variables
	public GameObject m_BluePlayerController;				// Contains UnitController Script
	public GameObject m_RedPlayerController; 				// Contains UnitController Script
	UnitController _blueUnitControllerScript;
	UnitController _redUnitControllerScript;
	
	// Region Variables
	public GameObject regionParent;
	public List<GameObject> regionList = new List<GameObject>();

	public GameObject currentRegion; 			// the region where the selected unit is on
	public GameObject selectedRegion;			// clicked region	

	
	// Unit Variables
	public GameObject Unit;
	public List<GameObject> unitList = new List<GameObject>();


	public int whosPlaying;						// for turn-based
	public bool StartOfGame;
	
	public int Turns;
	public int player1counter;
	public int player2counter;
	public bool EndOfGame;
	public bool endCount;

	E_PHASE thePhase;
	public bool OK;
	public GameObject m_mySlider;
	Slider _mySlider;

	public bool firstSetDone;
	public bool proceed;

	// VARIABLES USED FOR 3 MAIN PHASES ( REINFORCE, ATTACK, FORTIFY )
	public GameObject m_AttackFrom;
	public GameObject m_AttackTo;

	// Use this for initialization
	void Start () {
		StartOfGame = true;
		whosPlaying = 1;
		
		Turns = 12;
		EndOfGame = false;
		player1counter = 0;
		player2counter = 0;
		endCount = false;

		// Precautions
		if (m_sceneController == null) {
			m_sceneController = GameObject.Find ("SceneController");
		}
		if (mapParent == null) {
			mapParent = GameObject.Find("MapParent");
		}
		if (m_BluePlayerController == null) {
			m_BluePlayerController = GameObject.Find("BlueController");
		}
		if (m_RedPlayerController == null) {
			m_RedPlayerController = GameObject.Find("RedController");
		}
		if (regionParent == null) {
			regionParent = GameObject.Find("RegionParent");
		}
		if (m_mySlider == null) {
			m_mySlider = GameObject.Find("mySlider");
		}

		_mySlider = m_mySlider.GetComponent<Slider> ();

		m_mySlider.SetActive (false);

		_blueUnitControllerScript = m_BluePlayerController.GetComponent<UnitController> ();
		_redUnitControllerScript = m_RedPlayerController.GetComponent<UnitController> ();

		// Add Regions in List
		AddRegionInList ();
		// Set Number of Acquire_Remaining Units
		int dividedRegionSize = (int)(regionList.Count * 0.5f);
		_blueUnitControllerScript.Acquire_RemainingUnits = dividedRegionSize;
		_redUnitControllerScript.Acquire_RemainingUnits = dividedRegionSize;

		_blueUnitControllerScript.Deploy_RemainingUnits = 7;
		_redUnitControllerScript.Deploy_RemainingUnits = 7;

		thePhase = E_PHASE.ACQUIRE;
		OK = false;
		firstSetDone = false;
	}   
	
	// Update is called once per frame
	void Update () {

		if (EndOfGame == false) {

			PhaseTurns();
			
			foreach (GameObject unit in unitList) {
				UpdateTileOwnership (unit);
			}

			checkNeighbors ();
		
			if (Turns <= 0 || (player2counter <= 0 || player1counter <= 0) && StartOfGame == false)
			{
				EndOfGame = true;
			}

			// TO CANCEL ATTACK SELECTIONS
			if (Input.GetKeyDown("space") && thePhase == E_PHASE.ATTACK){
				currentRegion = null;
				m_AttackFrom = null;
				m_AttackTo = null;
				foreach(GameObject zRegion in regionList){
					zRegion.GetComponent<RegionScript>().canMoveTo = false;
					zRegion.GetComponent<RegionScript>().isSelected = false;
				}
			}

			// TO ATTACK
			if (Input.GetKeyDown("a") && thePhase == E_PHASE.ATTACK){
				if(m_AttackFrom != null &&
				   m_AttackTo != null)
					m_AttackFrom.GetComponent<RegionScript>().unitOnRegion.GetComponent<theUnit>().Attack();
			}

		}
	}
	
	void SelectRegion(){
		if (regionList.Count > 0) {
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray,Mathf.Infinity); // Cast a ray towards Z-axis in 2D Space(X-Y Axes)
				//			Debug.Log (hitInfo.transform.gameObject.name);
				if(hitInfo){
					foreach (GameObject theRegion in regionList) {
						if (theRegion.name == hitInfo.transform.gameObject.name) {
							selectedRegion = hitInfo.transform.gameObject;
							RegionScript _selectedRegion = selectedRegion.GetComponent<RegionScript>();

							// ------------------------------------------------------------------------------------------------------------------------- //
							// ---------------------------------------------- **&&!! ACQUIRE PHASE !!&&** ---------------------------------------------- //
							// ------------------------------------------------------------------------------------------------------------------------- //

							if(StartOfGame == true && thePhase == E_PHASE.ACQUIRE){
								if (selectedRegion.GetComponent<RegionScript> ().hasUnit == false) { // to prevent starting on the same regionee

									GameObject m_Unit = (GameObject)Instantiate (Unit);
									theUnit _Unit = m_Unit.GetComponent<theUnit>();

									//theUnit.transform.position = new Vector3(selectedRegion.transform.position.x,selectedRegion.transform.position.y, -0.1f);
									m_Unit.transform.position = selectedRegion.transform.position;
									_Unit.posX = (float)m_Unit.transform.position.x;
									_Unit.posY = (float)m_Unit.transform.position.y;
									_Unit.ID = whosPlaying;
									if(m_Unit.GetComponent<theUnit> ().ID == 1)
									{
										player1counter ++;
										m_Unit.tag = "unit_Player1";
										m_Unit.transform.parent = m_BluePlayerController.transform;
										_blueUnitControllerScript.m_unitList.Add(m_Unit);
										_blueUnitControllerScript.Acquire_RemainingUnits -- ;
										whosPlaying = 2;
										///m_Unit.GetComponent<theUnit>().numberOfTrainableUnit = (int) (2 * player1counter) + 1;
									} else if(m_Unit.GetComponent<theUnit> ().ID == 2)
									{
										player2counter ++;
										m_Unit.tag = "unit_Player2";
										m_Unit.transform.parent = m_RedPlayerController.transform;
										_redUnitControllerScript.m_unitList.Add(m_Unit);
										_redUnitControllerScript.Acquire_RemainingUnits -- ; 
										whosPlaying = 1;
										//m_Unit.GetComponent<theUnit>().numberOfTrainableUnit = (int) (2 * player2counter) + 1;
									}
									//m_Unit.transform.parent = m_sceneController.transform;
									//m_sceneController.GetComponent<SceneController> ().objectsInHierarchy.Add (m_Unit);
									unitList.Add (m_Unit);
									selectedRegion.GetComponent<RegionScript> ().unitOnRegion = m_Unit;
									selectedRegion.GetComponent<RegionScript> ().hasUnit = true;
									selectedRegion = null;
								}

							} 
							// ------------------------------------------------------------------------------------------------------------------------- //
							// ---------------------------------------------- **&&!! DEPLOY  PHASE !!&&** ---------------------------------------------- //
							// ------------------------------------------------------------------------------------------------------------------------- //

							else if (StartOfGame == false && thePhase == E_PHASE.DEPLOY){
								//if(selectedRegion != currentRegion){

								switch(whosPlaying){
								case 1:
									if(_selectedRegion.region_Owner == 1){
										_selectedRegion.isSelected = true;
										_selectedRegion.unitOnRegion.GetComponent<theUnit>().numberOfUnits++;
										_blueUnitControllerScript.Deploy_RemainingUnits--;
										whosPlaying = 2;
									}
									else{
										_selectedRegion.isSelected = false;
									}
									break;
								case 2:
									if(_selectedRegion.region_Owner == 2){
										_selectedRegion.isSelected = true;
										_selectedRegion.unitOnRegion.GetComponent<theUnit>().numberOfUnits++;
										_redUnitControllerScript.Deploy_RemainingUnits--;
										whosPlaying = 1;
									}
									else{
										_selectedRegion.isSelected = false;
									}
									break;
								}
							} 

							// ------------------------------------------------------------------------------------------------------------------------- //
							// --------------------------------------------- **&&!! REINFORCE PHASE !!&&** --------------------------------------------- //
							// ------------------------------------------------------------------------------------------------------------------------- //
							
							else if (StartOfGame == false && thePhase == E_PHASE.REINFORCE){
								//if(selectedRegion != currentRegion){
								
								switch(whosPlaying){
								case 1:
									if(_selectedRegion.region_Owner == 1){
										if(_blueUnitControllerScript.Deploy_RemainingUnits > 0){
											_selectedRegion.isSelected = true;
											_selectedRegion.unitOnRegion.GetComponent<theUnit>().numberOfUnits++;
											_blueUnitControllerScript.Deploy_RemainingUnits--;
										}
										else if(_blueUnitControllerScript.Deploy_RemainingUnits <= 0)
										{
											thePhase = E_PHASE.ATTACK;
										}
									}
									else{
										_selectedRegion.isSelected = false;
									}
									break;
								case 2:
									if(_selectedRegion.region_Owner == 2){
										if(_redUnitControllerScript.Deploy_RemainingUnits > 0){
											_selectedRegion.isSelected = true;
											_selectedRegion.unitOnRegion.GetComponent<theUnit>().numberOfUnits++;
											_redUnitControllerScript.Deploy_RemainingUnits--;
										}
										else if(_redUnitControllerScript.Deploy_RemainingUnits <= 0)
										{
											thePhase = E_PHASE.ATTACK;
										}
									}
									else{
										_selectedRegion.isSelected = false;
									}
									break;
								}
							} 

							// ------------------------------------------------------------------------------------------------------------------------- //
							// ---------------------------------------------- **&&!! ATTACK  PHASE !!&&** ---------------------------------------------- //
							// ------------------------------------------------------------------------------------------------------------------------- //

							else if(StartOfGame == false && thePhase == E_PHASE.ATTACK){

								switch(whosPlaying){
								case 1:
									if(_selectedRegion.region_Owner == 1 && m_AttackFrom == null && _selectedRegion.unitOnRegion.GetComponent<theUnit>().numberOfUnits > 1){
										_selectedRegion.isSelected = true;
										currentRegion = selectedRegion;
										m_AttackFrom = currentRegion;
										//whosPlaying = 2;
									}
									else if(m_AttackFrom != null){
										if(selectedRegion != m_AttackFrom && selectedRegion.GetComponent<RegionScript>().canMoveTo == true && _selectedRegion.region_Owner == 2 ){
											m_AttackTo = selectedRegion;
											_selectedRegion.isSelected = true;
										}
										else{
											_selectedRegion.isSelected = false;
											m_AttackTo = null;
										}
									}
									else{
										_selectedRegion.isSelected = false;

									}
									break;
								case 2:
									if(_selectedRegion.region_Owner == 2 && m_AttackFrom == null){
										_selectedRegion.isSelected = true;
										currentRegion = selectedRegion;
										m_AttackFrom = currentRegion;
										//whosPlaying = 2;
									}
									else if(m_AttackFrom != null){
										if(selectedRegion != m_AttackFrom && selectedRegion.GetComponent<RegionScript>().canMoveTo == true && _selectedRegion.region_Owner == 1 ){
											m_AttackTo = selectedRegion;
											_selectedRegion.isSelected = true;
										}
										else{
											_selectedRegion.isSelected = false;
											m_AttackTo = null;
										}
									}
									
									else{
										_selectedRegion.isSelected = false;
									}
									break;
								}
							}

							// ------------------------------------------------------------------------------------------------------------------------- //
							// ---------------------------------------------- **&&!! FORTIFY PHASE !!&&** ---------------------------------------------- //
							// ------------------------------------------------------------------------------------------------------------------------- //

							else if(StartOfGame == false && thePhase == E_PHASE.FORTIFY){
								switch(whosPlaying){
								case 1:
									if(_selectedRegion.region_Owner == 1 && m_AttackFrom == null && _selectedRegion.unitOnRegion.GetComponent<theUnit>().numberOfUnits > 1){
										_selectedRegion.isSelected = true;
										m_AttackFrom = selectedRegion;
									}
									else if(m_AttackFrom != null){
										if(selectedRegion != m_AttackFrom && _selectedRegion.region_Owner != 2 ){
											m_AttackTo = selectedRegion;
											_selectedRegion.isSelected = true;

											if(m_AttackFrom != null && m_AttackTo != null){
												Debug.Log("FROM AND TO IS NOT NULL, PROCEED!");
												proceed = true;
											}
										}
										else{
											_selectedRegion.isSelected = false;
											m_AttackTo = null;
										}
									}
//									else{
//										_selectedRegion.isSelected = false;
//									}
									break;

								case 2:
									if(_selectedRegion.region_Owner == 2 && m_AttackFrom == null && _selectedRegion.unitOnRegion.GetComponent<theUnit>().numberOfUnits > 1){
										_selectedRegion.isSelected = true;
										m_AttackFrom = selectedRegion;
									}
									else if(m_AttackFrom != null){
										if(selectedRegion != m_AttackFrom && _selectedRegion.region_Owner != 1 ){
											m_AttackTo = selectedRegion;
											_selectedRegion.isSelected = true;

											if(m_AttackFrom != null && m_AttackTo != null){
												Debug.Log("FROM AND TO IS NOT NULL, PROCEED!");
												proceed = true;
											}
										}
										else{
											_selectedRegion.isSelected = false;
											m_AttackTo = null;
										}
									}
//									else{
//										_selectedRegion.isSelected = false;
//									}
									break;
								}
							}

							else {
								if(selectedRegion != currentRegion)
									selectedRegion.GetComponent<RegionScript> ().isSelected = true;
								else
									selectedRegion = null;
							}
						} else if (hitInfo.transform.gameObject.name != theRegion.name) {
							//Debug.Log ("not Selected");
							theRegion.GetComponent<RegionScript> ().isSelected = false;
						}
					}
				}
			}
		}
	}

	void UpdateTileOwnership(GameObject goUnit){
		foreach (GameObject goTile in regionList) {
			if (goTile.GetComponent<RegionScript>().region_Owner == 0) {
				if (goTile.GetComponent<RegionScript> ().regionX == goUnit.GetComponent<theUnit> ().posX && goTile.GetComponent<RegionScript> ().regionY == goUnit.GetComponent<theUnit> ().posY) {
					goTile.GetComponent<RegionScript> ().region_Owner = goUnit.GetComponent<theUnit>().ID;
				}
			}
		}
	}
	
	// THIS GONNA BE LONG!!!!!!
	void checkNeighbors(){
		foreach(GameObject regionTile in regionList)
		{
			regionTile.GetComponent<RegionScript>().canMoveTo = false;
		}
		if (currentRegion != null) {
			
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

		if (EndOfGame == true){
			if(player1counter > player2counter){
				GUI.Box (new Rect (Screen.width * 0.45f, 0, 180, 25), "Player 1 WINS!");
			} else if(player1counter < player2counter){
				GUI.Box (new Rect (Screen.width * 0.45f, 0, 180, 25), "Player 2 WINS!");
			} else if(player1counter == player2counter){
				GUI.Box (new Rect (Screen.width * 0.45f, 0, 180, 25), "DRAW!!");
			}
		} else {
			if(whosPlaying == 1){
				GUI.Box (new Rect (Screen.width * 0.45f, 0, 180, 50), "BLUE's Turn");
			}else if( whosPlaying == 2){
				GUI.Box (new Rect (Screen.width * 0.45f, 0, 180, 50), "RED's Turn");
			}
			GUI.Label (new Rect (Screen.width * 0.476f, 20, 180, 25), "Turns Left: " + Turns);

		}
		GUI.Box (new Rect (Screen.width * 0.37f, 100, 500, 25), "Phase: " + thePhase);
		if (thePhase == E_PHASE.DEPLOY) {
			GUI.Box (new Rect (Screen.width * 0.37f, 150, 500, 25), "REMAINING UNITS " + _redUnitControllerScript.Deploy_RemainingUnits);
		}
	}

	void PhaseTurns(){
		switch (thePhase) {

		case E_PHASE.ACQUIRE:
			if(_redUnitControllerScript.Acquire_RemainingUnits > 0){
				SelectRegion();
			}
			else{
				StartOfGame = false;
				thePhase = E_PHASE.DEPLOY;
			}
			break;

		case E_PHASE.DEPLOY:
			if(_redUnitControllerScript.Deploy_RemainingUnits > 0){
				SelectRegion();
			}
			else{
				selectedRegion = null;
				thePhase = E_PHASE.ATTACK;
			}
			break;
			
		case E_PHASE.REINFORCE:
			SelectRegion();
			break;

		case E_PHASE.ATTACK:
			SelectRegion();
			ActionForResult(m_sceneController.GetComponent<SceneController>().battleResults);
			break;

		case E_PHASE.FORTIFY:
			SelectRegion();
			if(proceed == true){
				AddMinusUnits();
			}
			break;

		}
	}

	void ActionForResult(int result){
		switch (result) {
		case 1:
			Win ();
			break;

		case 2:
			Lose ();
			break;

		case 3:
			Draw ();
			break;
		}
	}

	void Win(){
		// Set To's tag and ID as the winner's
		// Take Remaining Units in From to set amount of transferable Units. must not be >= to the total number of remaining units
		// Set into the Slider
		if (OK == false) {
			m_mySlider.SetActive(true);
			_mySlider.maxValue = m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits - 1;
			_mySlider.minValue = 1;
			GameObject m_tempPanelObj = _mySlider.transform.FindChild("NumberPanel").gameObject;
			GameObject m_tempPanelTextObj = m_tempPanelObj.transform.FindChild("NumberText").gameObject;
			m_tempPanelTextObj.GetComponent<Text>().text = _mySlider.value.ToString();
		}

		if (OK == true) {

			m_AttackTo.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
			m_AttackTo.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
			m_AttackTo.GetComponent<RegionScript> ().region_Owner = m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;

			// Add to "To GameObject", Minus from "From GameObject"
			m_AttackTo.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = (int)_mySlider.value;
			m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits -= (int)_mySlider.value;

			if(m_AttackTo.GetComponent<RegionScript>().region_Owner == 1){
				player1counter ++;
				player2counter --;
			}
			else if(m_AttackTo.GetComponent<RegionScript>().region_Owner == 2){
				player2counter ++;
				player1counter --;
			}

			m_sceneController.GetComponent<SceneController>().battleResults = 0;
			m_mySlider.SetActive(false);
			OK = false;
			m_AttackFrom = null;
			m_AttackTo = null;
			selectedRegion = null;
			currentRegion = null;
			thePhase = E_PHASE.FORTIFY;
		}
	}

	void Lose(){

		m_sceneController.GetComponent<SceneController>().battleResults = 0;
		m_AttackFrom = null;
		m_AttackTo = null;
		selectedRegion = null;
		currentRegion = null;
		thePhase = E_PHASE.FORTIFY;

	}

	void Draw(){
		m_sceneController.GetComponent<SceneController>().battleResults = 0;
		m_AttackFrom = null;
		m_AttackTo = null;
		selectedRegion = null;
		currentRegion = null;
		thePhase = E_PHASE.FORTIFY;
	}
	// for Button Sake
	public void set_OK_True()
	{
		OK = true;
		Debug.Log ("BOOL OK PRESSED!");
	}


	void AddMinusUnits(){
		if (OK == false) {
			m_mySlider.SetActive(true);
			_mySlider.maxValue = m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits - 1;
			_mySlider.minValue = 1;
			
			GameObject m_tempPanelObj = _mySlider.transform.FindChild("NumberPanel").gameObject;
			GameObject m_tempPanelTextObj = m_tempPanelObj.transform.FindChild("NumberText").gameObject;
			m_tempPanelTextObj.GetComponent<Text>().text = _mySlider.value.ToString();
		}
		
		if (OK == true) {
			// Add to "To GameObject", Minus from "From GameObject"
			m_AttackTo.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits += (int)_mySlider.value;
			m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits -= (int)_mySlider.value;
			m_mySlider.SetActive(false);
			OK = false;
			m_AttackFrom = null;
			m_AttackTo = null;
			selectedRegion = null;
			currentRegion = null;
			proceed = false;
			
			if(firstSetDone == false){
				if(whosPlaying == 1){
					whosPlaying = 2;
				}
				else if(whosPlaying == 2){
					whosPlaying = 1;
				}
				thePhase = E_PHASE.ATTACK;
				firstSetDone = true;
			}

			else if(firstSetDone == true){
				if(whosPlaying == 1){
					_redUnitControllerScript.Deploy_RemainingUnits = 3; // Reinforce 5 Units
					whosPlaying = 2;
				}
				else if(whosPlaying == 2){
					_blueUnitControllerScript.Deploy_RemainingUnits = 3; // Reinforce 5 Units
					Turns--;
					whosPlaying = 1;
				}
				thePhase = E_PHASE.REINFORCE;
			}
		}
	}
}
