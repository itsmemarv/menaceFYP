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
	public bool SliderActive;
	
	public bool firstSetDone;
	public bool proceed;
	public bool inAddMinus;
	
	// VARIABLES USED FOR 3 MAIN PHASES ( REINFORCE, ATTACK, FORTIFY )
	public GameObject m_AttackFrom;
	public GameObject m_AttackTo;
	
	// Phase Text
	public Text PHASETEXT;
	// The Edges
	public GameObject CircleOut;
	
	// Variables for Animation
	// Phase Background
	public GameObject m_PhaseBG;
	private Animator _PhaseBGAnim;
	// Phase Text
	public GameObject m_PhaseTextGO;
	private Animator _PhaseTextGOAnim;
	
	// Variables for Text
	// Turns Left Text
	public GameObject TurnsLeftTEXT;
	private Text text_TurnsLeftTEXT;
	// Remaining Units Text
	public GameObject RemUnitsTEXT;
	private Text text_RemUnitsTEXT;
	
	public bool BlueAllHaveMoreThan1Unit;
	public bool RedAllHaveMoreThan1Unit;
	
	Color myBlueColor;
	Color myRedColor;

	public AudioClip myClip;
	public AudioClip okClip;
	public AudioSource audioSelectRegion;

	public AudioClip phaseClip;
	public AudioSource audioChangePhase;

	public AudioClip winClip;
	public AudioClip losedrawClip;
	public AudioSource audioBattleResults;

	public bool playedsfx;

	public int DEPLOYABLES;
	public int REINFORCEMENTS;

	public GameObject AttackButton;
	public GameObject CancelButton;
	public GameObject SkipButton;

	// Use this for initialization
	void Start () {
		StartOfGame = true;
		whosPlaying = 1;
		myBlueColor = new Color (0.043f, 0.043f, 0.494f);
		myRedColor =  new Color (0.494f, 0.043f, 0.043f);
		CircleOut.GetComponent<SpriteRenderer>().material.color = myBlueColor;
		Turns = 12;
		EndOfGame = false;
		player1counter = 0;
		player2counter = 0;
		endCount = false;
		playedsfx = false;
		DEPLOYABLES = 7;
		REINFORCEMENTS = 5;
		
		_PhaseBGAnim = m_PhaseBG.GetComponent<Animator> ();
		_PhaseTextGOAnim = m_PhaseTextGO.GetComponent<Animator> ();
		
		_mySlider = m_mySlider.GetComponent<Slider> ();
		m_mySlider.SetActive (false);
		SliderActive = false;
		
		_blueUnitControllerScript = m_BluePlayerController.GetComponent<UnitController> ();
		_redUnitControllerScript = m_RedPlayerController.GetComponent<UnitController> ();
		
		text_TurnsLeftTEXT = TurnsLeftTEXT.GetComponent<Text> ();
		text_RemUnitsTEXT = RemUnitsTEXT.GetComponent<Text> ();

		// Enable only if in DEPLOY PHASE
		RemUnitsTEXT.SetActive (false);
		
		// Add Regions in List
		AddRegionInList ();
		// Set Number of Acquire_Remaining Units
		int dividedRegionSize = (int)(regionList.Count * 0.5f);
		_blueUnitControllerScript.Acquire_RemainingUnits = dividedRegionSize;
		_redUnitControllerScript.Acquire_RemainingUnits = dividedRegionSize;
		
		_blueUnitControllerScript.Deploy_RemainingUnits = DEPLOYABLES;
		_redUnitControllerScript.Deploy_RemainingUnits = DEPLOYABLES;
		
		thePhase = E_PHASE.ACQUIRE;
		_PhaseBGAnim.SetTrigger ("play");
		_PhaseTextGOAnim.SetTrigger ("playtext");
		PHASETEXT.text = thePhase.ToString();
		OK = false;
		firstSetDone = false;
		proceed = false;
		inAddMinus = false;
		BlueAllHaveMoreThan1Unit = false;
		RedAllHaveMoreThan1Unit = false;

		audioSelectRegion = (AudioSource)gameObject.AddComponent<AudioSource> ();
		audioSelectRegion.clip = myClip;
		audioSelectRegion.loop = false;
		audioSelectRegion.playOnAwake = false;

		audioChangePhase = (AudioSource)gameObject.AddComponent<AudioSource> ();
		audioChangePhase.clip = phaseClip;
		audioChangePhase.loop = false;
		audioChangePhase.playOnAwake = false;

		audioBattleResults = (AudioSource)gameObject.AddComponent<AudioSource> ();
		audioBattleResults.clip = winClip;
		audioBattleResults.loop = false;
		audioBattleResults.playOnAwake = false;

		if (StartOfGame == true) {
			audioChangePhase.PlayOneShot (phaseClip);
		}
		AttackButton.GetComponent<Button> ().interactable = false;
		CancelButton.GetComponent<Button> ().interactable = false;
		SkipButton.GetComponent<Button> ().interactable = false;
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

			// TO ATTACK
			if (thePhase == E_PHASE.ATTACK){
				AttackButton.GetComponent<Button> ().interactable = true;
			}
			else if (thePhase != E_PHASE.ATTACK){
				AttackButton.GetComponent<Button> ().interactable = false;
			}

			// TO CANCEL ATTACK SELECTIONS
			if ((thePhase == E_PHASE.ATTACK || thePhase == E_PHASE.FORTIFY)){
				CancelButton.GetComponent<Button> ().interactable = true;
			}
			else if ((thePhase != E_PHASE.ATTACK || thePhase != E_PHASE.FORTIFY)){
				AttackButton.GetComponent<Button> ().interactable = false;
			}

			// TO SKIP TURN
			if(firstSetDone == true){
				SkipButton.GetComponent<Button> ().interactable = true;
			}

			// TURNS LEFT TEXT
			text_TurnsLeftTEXT.text = "Turns Left: " + Turns.ToString();

			// DEPLOYABLE/REINFORCEMENTS TEXT
			if (thePhase == E_PHASE.DEPLOY){
				RemUnitsTEXT.SetActive (true);
				if(whosPlaying == 1){
					text_RemUnitsTEXT.text = "Blue Deploy Units: " + _blueUnitControllerScript.Deploy_RemainingUnits.ToString();
				}
				else if (whosPlaying == 2){
					text_RemUnitsTEXT.text = "Red Deploy Units: " + _redUnitControllerScript.Deploy_RemainingUnits.ToString();
				}
			}
			else if(thePhase == E_PHASE.REINFORCE){
				RemUnitsTEXT.SetActive (true);
				if(whosPlaying == 1){
					text_RemUnitsTEXT.text = "Blue Reinforce Units: " + _blueUnitControllerScript.Deploy_RemainingUnits.ToString();
				}
				else if (whosPlaying == 2){
					text_RemUnitsTEXT.text = "Red Reinforce Units: " + _redUnitControllerScript.Deploy_RemainingUnits.ToString();
				}
			}
			else{
				RemUnitsTEXT.SetActive (false);
			}

			if(Input.GetKeyDown("r") && firstSetDone == true){
				SkipTurn();
			}

//			if(Input.GetKeyDown("p")){
//				TestEndGame();
//			}


		}
		
	}
	
	void SelectRegion(){
		if (regionList.Count > 0 && SliderActive == false) {
			if (Input.GetMouseButtonDown (0)) {

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray, Mathf.Infinity); // Cast a ray towards Z-axis in 2D Space(X-Y Axes)
				//			Debug.Log (hitInfo.transform.gameObject.name);
				if (hitInfo) {
					foreach (GameObject theRegion in regionList) {
						if (theRegion.name == hitInfo.transform.gameObject.name) {
							selectedRegion = hitInfo.transform.gameObject;
							RegionScript _selectedRegion = selectedRegion.GetComponent<RegionScript> ();
							
							// ------------------------------------------------------------------------------------------------------------------------- //
							// ---------------------------------------------- **&&!! ACQUIRE PHASE !!&&** ---------------------------------------------- //
							// ------------------------------------------------------------------------------------------------------------------------- //
							
							if (StartOfGame == true && thePhase == E_PHASE.ACQUIRE) {

								proceed = false;
								if (selectedRegion.GetComponent<RegionScript> ().hasUnit == false) { // to prevent starting on the same regionee
									audioSelectRegion.PlayOneShot(myClip);
									GameObject m_Unit = (GameObject)Instantiate (Unit);
									theUnit _Unit = m_Unit.GetComponent<theUnit> ();
									
									//theUnit.transform.position = new Vector3(selectedRegion.transform.position.x,selectedRegion.transform.position.y, -0.1f);
									m_Unit.transform.position = selectedRegion.transform.position;
									_Unit.posX = (float)m_Unit.transform.position.x;
									_Unit.posY = (float)m_Unit.transform.position.y;
									_Unit.ID = whosPlaying;
									if (m_Unit.GetComponent<theUnit> ().ID == 1) {
										player1counter ++;
										m_Unit.tag = "unit_Player1";
										m_Unit.transform.parent = m_BluePlayerController.transform;
										_blueUnitControllerScript.m_unitList.Add (m_Unit);
										_blueUnitControllerScript.Acquire_RemainingUnits --;
										//selectedRegion.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(0.173f, 0.157f, 0.639f));
										selectedRegion.GetComponent<RegionScript> ().origColor = new Color (0.173f, 0.157f, 0.639f);
										whosPlaying = 2;
										CircleOut.GetComponent<SpriteRenderer> ().material.color = myRedColor;
										///m_Unit.GetComponent<theUnit>().numberOfTrainableUnit = (int) (2 * player1counter) + 1;
									} else if (m_Unit.GetComponent<theUnit> ().ID == 2) {
										player2counter ++;
										m_Unit.tag = "unit_Player2";
										m_Unit.transform.parent = m_RedPlayerController.transform;
										_redUnitControllerScript.m_unitList.Add (m_Unit);
										_redUnitControllerScript.Acquire_RemainingUnits --; 
										//selectedRegion.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(0.639f, 0.173f, 0.157f));
										selectedRegion.GetComponent<RegionScript> ().origColor = new Color (0.639f, 0.173f, 0.157f);
										whosPlaying = 1;
										CircleOut.GetComponent<SpriteRenderer> ().material.color = myBlueColor;
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
							
							else if (StartOfGame == false && thePhase == E_PHASE.DEPLOY) {
								proceed = false;
								//if(selectedRegion != currentRegion){
								
								switch (whosPlaying) {
								case 1:
									if (_selectedRegion.region_Owner == 1) {
										audioSelectRegion.PlayOneShot(myClip);
										_selectedRegion.isSelected = true;
										_selectedRegion.unitOnRegion.GetComponent<theUnit> ().numberOfUnits++;
										_blueUnitControllerScript.Deploy_RemainingUnits--;
										whosPlaying = 2;
										CircleOut.GetComponent<SpriteRenderer> ().material.color = myRedColor;
									} else {
										_selectedRegion.isSelected = false;
									}
									break;
								case 2:
									if (_selectedRegion.region_Owner == 2) {
										audioSelectRegion.PlayOneShot(myClip);
										_selectedRegion.isSelected = true;
										_selectedRegion.unitOnRegion.GetComponent<theUnit> ().numberOfUnits++;
										_redUnitControllerScript.Deploy_RemainingUnits--;
										whosPlaying = 1;
										CircleOut.GetComponent<SpriteRenderer> ().material.color = myBlueColor;
									} else {
										_selectedRegion.isSelected = false;
									}
									break;
								}
							} 
							
							// ------------------------------------------------------------------------------------------------------------------------- //
							// --------------------------------------------- **&&!! REINFORCE PHASE !!&&** --------------------------------------------- //
							// ------------------------------------------------------------------------------------------------------------------------- //
							
							else if (StartOfGame == false && thePhase == E_PHASE.REINFORCE) {
								proceed = false;
								//if(selectedRegion != currentRegion){
								switch (whosPlaying) {
								case 1:
									if (_selectedRegion.region_Owner == 1) {
										audioSelectRegion.PlayOneShot(myClip);
										if (_blueUnitControllerScript.Deploy_RemainingUnits > 0) {
											_selectedRegion.isSelected = true;
											_selectedRegion.unitOnRegion.GetComponent<theUnit> ().numberOfUnits++;
											_blueUnitControllerScript.Deploy_RemainingUnits--;
											if (_blueUnitControllerScript.Deploy_RemainingUnits <= 0) {
												proceed = true;
											}
										}
									} else {
										_selectedRegion.isSelected = false;
									}
									break;
								case 2:
									if (_selectedRegion.region_Owner == 2) {
										audioSelectRegion.PlayOneShot(myClip);
										if (_redUnitControllerScript.Deploy_RemainingUnits > 0) {
											_selectedRegion.isSelected = true;
											_selectedRegion.unitOnRegion.GetComponent<theUnit> ().numberOfUnits++;
											_redUnitControllerScript.Deploy_RemainingUnits--;
											if (_redUnitControllerScript.Deploy_RemainingUnits <= 0) {
												proceed = true;
											}
										}
									} else {
										_selectedRegion.isSelected = false;
									}
									break;
								}
							} 
							
							// ------------------------------------------------------------------------------------------------------------------------- //
							// ---------------------------------------------- **&&!! ATTACK  PHASE !!&&** ---------------------------------------------- //
							// ------------------------------------------------------------------------------------------------------------------------- //
							
							else if (StartOfGame == false && thePhase == E_PHASE.ATTACK) {
								proceed = false;
								switch (whosPlaying) {
								case 1:
									if (_selectedRegion.region_Owner == 1 && m_AttackFrom == null && _selectedRegion.unitOnRegion.GetComponent<theUnit> ().numberOfUnits > 1) 
									{
										audioSelectRegion.PlayOneShot(myClip);
										_selectedRegion.isSelected = true;
										currentRegion = selectedRegion;
										m_AttackFrom = currentRegion;
										//whosPlaying = 2;
									} else if (m_AttackFrom != null) {
										if (selectedRegion != m_AttackFrom && selectedRegion.GetComponent<RegionScript> ().canMoveTo == true && 
											 _selectedRegion.region_Owner == 2) {
											audioSelectRegion.PlayOneShot(myClip);
											m_AttackTo = selectedRegion;
											_selectedRegion.isSelected = true;
										} else {
											_selectedRegion.isSelected = false;
											m_AttackTo = null;
										}
									} else {
										_selectedRegion.isSelected = false;
										
									}
									break;
								case 2:
									if (_selectedRegion.region_Owner == 2 && m_AttackFrom == null && _selectedRegion.unitOnRegion.GetComponent<theUnit> ().numberOfUnits > 1) 
									{
										audioSelectRegion.PlayOneShot(myClip);
										_selectedRegion.isSelected = true;
										currentRegion = selectedRegion;
										m_AttackFrom = currentRegion;
										//whosPlaying = 2;
									} else if (m_AttackFrom != null) {
										if (selectedRegion != m_AttackFrom && selectedRegion.GetComponent<RegionScript> ().canMoveTo == true && 
										    _selectedRegion.region_Owner == 1) {
											audioSelectRegion.PlayOneShot(myClip);
											m_AttackTo = selectedRegion;
											_selectedRegion.isSelected = true;
										} else {
											_selectedRegion.isSelected = false;
											m_AttackTo = null;
										}
									} else {
										_selectedRegion.isSelected = false;
									}
									break;
								}
							}
							
							// ------------------------------------------------------------------------------------------------------------------------- //
							// ---------------------------------------------- **&&!! FORTIFY PHASE !!&&** ---------------------------------------------- //
							// ------------------------------------------------------------------------------------------------------------------------- //
							
							else if (StartOfGame == false && thePhase == E_PHASE.FORTIFY) {
								proceed = false;
								switch (whosPlaying) {
								case 1:
									if (_selectedRegion.region_Owner == 1 && m_AttackFrom == null && _selectedRegion.unitOnRegion.GetComponent<theUnit> ().numberOfUnits > 1) 
									{
										audioSelectRegion.PlayOneShot(myClip);
										_selectedRegion.isSelected = true;
										m_AttackFrom = selectedRegion;
									} else if (m_AttackFrom != null) {
										if (selectedRegion != m_AttackFrom && _selectedRegion.region_Owner != 2) {
											audioSelectRegion.PlayOneShot(myClip);
											m_AttackTo = selectedRegion;
											_selectedRegion.isSelected = true;
											
											if (m_AttackFrom != null && m_AttackTo != null) {
												Debug.Log ("FROM AND TO IS NOT NULL, go to AddMinus!");
												inAddMinus = true;
											}
										} else {
											_selectedRegion.isSelected = false;
											m_AttackTo = null;
										}
									}
									//									else{
									//										_selectedRegion.isSelected = false;
									//									}
									break;
									
								case 2:
									if (_selectedRegion.region_Owner == 2 && m_AttackFrom == null && _selectedRegion.unitOnRegion.GetComponent<theUnit> ().numberOfUnits > 1) 
									{
										audioSelectRegion.PlayOneShot(myClip);
										_selectedRegion.isSelected = true;
										m_AttackFrom = selectedRegion;
									} else if (m_AttackFrom != null) {
										if (selectedRegion != m_AttackFrom && _selectedRegion.region_Owner != 1) {
											audioSelectRegion.PlayOneShot(myClip);
											m_AttackTo = selectedRegion;
											_selectedRegion.isSelected = true;
											
											if (m_AttackFrom != null && m_AttackTo != null) {
												Debug.Log ("FROM AND TO IS NOT NULL, go to AddMinus!");
												inAddMinus = true;
											}
										} else {
											_selectedRegion.isSelected = false;
											m_AttackTo = null;
										}
									}
									//									else{
									//										_selectedRegion.isSelected = false;
									//									}
									break;
								}
							} else {
								if (selectedRegion != currentRegion)
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
				if (goTile.GetComponent<RegionScript> ().regionX == goUnit.GetComponent<theUnit> ().posX && goTile.GetComponent<RegionScript> ().regionY == goUnit.GetComponent<theUnit> ().posY) 
				{
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
				regionList [7].GetComponent<RegionScript> ().canMoveTo = true; // middle_7
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
				regionList [4].GetComponent<RegionScript> ().canMoveTo = true; // middle_1
				break;
			case 3: // west_4
				regionList [2].GetComponent<RegionScript> ().canMoveTo = true; // west_3
				regionList [1].GetComponent<RegionScript> ().canMoveTo = true; // west_2
				break;
				
				// .. MIDDLE CONTINENT .. // 
			case 4: // middle_1
				regionList [5].GetComponent<RegionScript> ().canMoveTo = true; // middle_2
				regionList [6].GetComponent<RegionScript> ().canMoveTo = true; // middle_3
				regionList [2].GetComponent<RegionScript> ().canMoveTo = true; // west_2
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
				regionList [0].GetComponent<RegionScript> ().canMoveTo = true; // west_1
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
				regionList [14].GetComponent<RegionScript> ().canMoveTo = true; // east_3
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
				regionList [10].GetComponent<RegionScript> ().canMoveTo = true; // middle_7
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
		
	void PhaseTurns(){
		switch (thePhase) {
			
		case E_PHASE.ACQUIRE:
			if(_redUnitControllerScript.Acquire_RemainingUnits > 0){
				SelectRegion();
			}
			else{
				StartOfGame = false;
				thePhase = E_PHASE.DEPLOY;
				audioChangePhase.PlayOneShot(phaseClip);
				PHASETEXT.text = thePhase.ToString();
				
				_PhaseBGAnim.SetTrigger ("play");
				_PhaseTextGOAnim.SetTrigger ("playtext");
			}
			break;
			
		case E_PHASE.DEPLOY:
			if(_redUnitControllerScript.Deploy_RemainingUnits > 0){
				SelectRegion();
			}
			else{
				selectedRegion = null;
				thePhase = E_PHASE.ATTACK;
				audioChangePhase.PlayOneShot(phaseClip);
				PHASETEXT.text = thePhase.ToString();
				
				_PhaseBGAnim.SetTrigger ("play");
				_PhaseTextGOAnim.SetTrigger ("playtext");
			}
			break;
			
		case E_PHASE.REINFORCE:
			SelectRegion();
			if(proceed == true){
				thePhase = E_PHASE.ATTACK;
				audioChangePhase.PlayOneShot(phaseClip);
				PHASETEXT.text = thePhase.ToString();
				
				_PhaseBGAnim.SetTrigger ("play");
				_PhaseTextGOAnim.SetTrigger ("playtext");
			}
			break;
			
		case E_PHASE.ATTACK:
			SelectRegion();
			ActionForResult(m_sceneController.GetComponent<SceneController>().battleResults);
			break;
			
		case E_PHASE.FORTIFY:
			if(proceed == false && inAddMinus == false){
				SelectRegion();
			}
			
			if(inAddMinus == true){
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
		Debug.Log ("Played Win Sound");
		if (OK == false) {
			m_mySlider.SetActive(true);
			SliderActive = true;
			_mySlider.maxValue = m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits - 1;
			_mySlider.minValue = 1;
			GameObject m_tempPanelTextObj = _mySlider.transform.FindChild("NumberText").gameObject;
			m_tempPanelTextObj.GetComponent<Text>().text = _mySlider.value.ToString();
		}
		
		if (OK == true) {
			
			m_AttackTo.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
			m_AttackTo.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
			m_AttackTo.GetComponent<RegionScript> ().region_Owner = m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
			
			// Add to "To GameObject", Minus from "From GameObject"
			m_AttackTo.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = (int)_mySlider.value;
			if (m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits > 1) {
				m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits -= (int)_mySlider.value;
			} else {
				m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
			}
			if (m_AttackTo.GetComponent<RegionScript> ().region_Owner == 1) {
				//m_AttackTo.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(0.173f, 0.157f, 0.639f));
				m_AttackTo.GetComponent<RegionScript> ().origColor = new Color (0.173f, 0.157f, 0.639f);
				player1counter ++;
				player2counter --;
			} else if (m_AttackTo.GetComponent<RegionScript> ().region_Owner == 2) {
				//m_AttackTo.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(0.639f, 0.173f, 0.157f));
				m_AttackTo.GetComponent<RegionScript> ().origColor = new Color (0.639f, 0.173f, 0.157f);
				player2counter ++;
				player1counter --;
			}
			
			m_sceneController.GetComponent<SceneController> ().battleResults = 0;
			m_mySlider.SetActive (false);
			SliderActive = false;
			OK = false;
			m_AttackFrom = null;
			m_AttackTo = null;
			selectedRegion = null;
			currentRegion = null;
			audioBattleResults.PlayOneShot(winClip);
			ResultPhaseCheck ();
		}
	}
	
	void Lose(){

		m_sceneController.GetComponent<SceneController>().battleResults = 0;
		m_AttackFrom = null;
		m_AttackTo = null;
		selectedRegion = null;
		currentRegion = null;
		audioBattleResults.PlayOneShot(losedrawClip);
		ResultPhaseCheck();
		
	}
	
	void Draw(){
		m_sceneController.GetComponent<SceneController>().battleResults = 0;
		m_AttackFrom = null;
		m_AttackTo = null;
		selectedRegion = null;
		currentRegion = null;
		audioBattleResults.PlayOneShot(losedrawClip);
		ResultPhaseCheck ();
		
	}
	// for Button Sake
	public void set_OK_True()
	{
		audioSelectRegion.PlayOneShot(okClip);
		OK = true;
		Debug.Log ("BOOL OK PRESSED!");
	}
	
	
	void AddMinusUnits(){
		if (OK == false) {
			m_mySlider.SetActive(true);
			SliderActive = true;
			_mySlider.maxValue = m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits - 1;
			_mySlider.minValue = 1;
			
			GameObject m_tempPanelTextObj = _mySlider.transform.FindChild("NumberText").gameObject;
			m_tempPanelTextObj.GetComponent<Text>().text = _mySlider.value.ToString();
		}
		
		if (OK == true) {
			// Add to "To GameObject", Minus from "From GameObject"
			m_AttackTo.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits += (int)_mySlider.value;
			m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits -= (int)_mySlider.value;
			m_mySlider.SetActive(false);
			SliderActive = false;
			OK = false;
			m_AttackFrom = null;
			m_AttackTo = null;
			selectedRegion = null;
			currentRegion = null;
			proceed = false;
			
			
			if(firstSetDone == false){
				if(whosPlaying == 1){
					whosPlaying = 2;
					CircleOut.GetComponent<SpriteRenderer>().material.color = myRedColor;
				}
				else if(whosPlaying == 2){
					whosPlaying = 1;
					CircleOut.GetComponent<SpriteRenderer>().material.color = myBlueColor;
				}
				thePhase = E_PHASE.ATTACK;
				audioChangePhase.PlayOneShot(phaseClip);
				PHASETEXT.text = thePhase.ToString();
				_PhaseBGAnim.SetTrigger ("play");
				_PhaseTextGOAnim.SetTrigger ("playtext");
				firstSetDone = true;
				inAddMinus = false;
			}
			
			else if(firstSetDone == true){
				if(whosPlaying == 1){
					_redUnitControllerScript.Deploy_RemainingUnits = REINFORCEMENTS; // Reinforce 5 Units
					whosPlaying = 2;
					CircleOut.GetComponent<SpriteRenderer>().material.color = myRedColor;
				}
				else if(whosPlaying == 2){
					_blueUnitControllerScript.Deploy_RemainingUnits = REINFORCEMENTS; // Reinforce 5 Units
					Turns--;
					whosPlaying = 1;
					CircleOut.GetComponent<SpriteRenderer>().material.color = myBlueColor;
				}
				thePhase = E_PHASE.REINFORCE;
				audioChangePhase.PlayOneShot(phaseClip);
				PHASETEXT.text = thePhase.ToString();
				_PhaseBGAnim.SetTrigger ("play");
				_PhaseTextGOAnim.SetTrigger ("playtext");
				inAddMinus = false;
			}
		}
	}
	
	bool BlueUnitNumberCheck()
	{
		// All it needs to get to FORTIFY phase is that [one must have more than one unit] AND [the player has more than one region]
		// This function checks if it has more than 1 unit.
		int counter = 1;
		bool finishedCounting = false;
		foreach (GameObject mUnit in unitList) {
			if(mUnit.GetComponent<theUnit>().ID == 1){
				mUnit.GetComponent<theUnit>().HasMoreThan1UnitCheck();
				//Debug.Log("Red Checked Number of Units: " + counter);
				Debug.Log("Blue Checked Number of Units");
				if(mUnit.GetComponent<theUnit>().HasMoreThan1Unit == true){ 
					BlueAllHaveMoreThan1Unit = true; 
					finishedCounting = true;
					break;// One has more than one Unit, set AllHaveMoreThan1Unit as true(false if 1 has less than 1 unit)
				}
				else if(mUnit.GetComponent<theUnit>().HasMoreThan1Unit == false){
					BlueAllHaveMoreThan1Unit = false;	// One has less than 1 Unit, set AllHaveMoreThan1Unit as false(true if 1 has more than 1 unit)

				}
				counter ++;
				finishedCounting = true;
				//Debug.Log("RED mUnit count: " + counter );
			}
		}
		if (finishedCounting == true) {
			if (BlueAllHaveMoreThan1Unit == false) {
				Debug.Log("BlueAllHaveMoreThan1Unit == false");
				return false;
			} else if (BlueAllHaveMoreThan1Unit == true) {
				Debug.Log("BlueAllHaveMoreThan1Unit == true");
				return true;
			} else {
				return false;		
				}
		} else
			return false;
	}

	bool RedUnitNumberCheck()
	{
		// All it needs to get to FORTIFY phase is that [one must have more than one unit] AND [the player has more than one region]
		// This function checks if it has more than 1 unit.
		int counter = 1;
		bool finishedCounting = false;
		foreach (GameObject mUnit in unitList) {
			if(mUnit.GetComponent<theUnit>().ID == 2){
				mUnit.GetComponent<theUnit>().HasMoreThan1UnitCheck();
				//Debug.Log("Red Checked Number of Units: " + counter);
				if(mUnit.GetComponent<theUnit>().HasMoreThan1Unit == true){ 
					RedAllHaveMoreThan1Unit = true;
					finishedCounting = true;
					break; // One has more than one Unit, set AllHaveMoreThan1Unit as true(false if 1 has less than 1 unit)
				}
				else if(mUnit.GetComponent<theUnit>().HasMoreThan1Unit == false){
					RedAllHaveMoreThan1Unit = false;	// One has less than 1 Unit, set AllHaveMoreThan1Unit as false(true if 1 has more than 1 unit)
				}
				counter ++;
				finishedCounting = true;
				//Debug.Log("RED mUnit count: " + counter );
			}

		}
		if (finishedCounting == true) {
			if (RedAllHaveMoreThan1Unit == false) {
				//Debug.Log ("RedAllHaveMoreThan1Unit == false");
				finishedCounting = false;
				return false;
			} else if (RedAllHaveMoreThan1Unit == true) {
				finishedCounting = false;
				//Debug.Log ("RedAllHaveMoreThan1Unit == true");
				finishedCounting = false;
				return true;
			} else {
				return false;		
			}
		} else
			return false;
	}
	
	void ResultPhaseCheck(){
		if (whosPlaying == 1) {
			if (player1counter <= 1 || BlueUnitNumberCheck () == false) { 				// Blue has only 1 region left AND all regions only have 1 unit
				_redUnitControllerScript.Deploy_RemainingUnits = REINFORCEMENTS;						// Reinforce 5 Units
				whosPlaying = 2; 														// Set to Red Player Turn
				CircleOut.GetComponent<SpriteRenderer> ().material.color = myRedColor; 	// Change the BGEdges to red
				thePhase = E_PHASE.REINFORCE; 											// Set the Phase to REINFORCE (so that RED could reinforce)
				audioChangePhase.PlayOneShot(phaseClip);
				PHASETEXT.text = thePhase.ToString ();									// Change Text accrd to thePhase
				_PhaseBGAnim.SetTrigger ("play"); 										// Trigger Animation
				_PhaseTextGOAnim.SetTrigger ("playtext"); 								// Trigger Animation

				Debug.Log("blue count <= 1 && unitcheck false");
				
			} else if (player1counter > 1 && BlueUnitNumberCheck () == true) { 			// Blue has more than 1 region AND there are region/s with more than 1 unit
				if (EndOfGame == false) {												// Check if it is EndOfGame; IF NO,
					thePhase = E_PHASE.FORTIFY;											// Set the Phase to FORTIFY (so that BLUE could fortify)
					audioChangePhase.PlayOneShot(phaseClip);
					PHASETEXT.text = thePhase.ToString ();								// Change Text accrd to thePhase
					_PhaseBGAnim.SetTrigger ("play");									// Trigger Animation
					_PhaseTextGOAnim.SetTrigger ("playtext");							// Trigger Animation
					Debug.Log("blue counter > 1 && unitcheck true , eog false");
				} else if (EndOfGame == true) {											// IF YES,
					if (player1counter > player2counter) {								// Change Text to who wins accordingly
						audioChangePhase.PlayOneShot(phaseClip);
						PHASETEXT.text = "BLUE WINS!";									
						_PhaseBGAnim.SetTrigger ("play");								
						_PhaseTextGOAnim.SetTrigger ("playtext");
					} else if (player1counter < player2counter) {
						audioChangePhase.PlayOneShot(phaseClip);
						PHASETEXT.text = "RED WINS!";
						_PhaseBGAnim.SetTrigger ("play");
						_PhaseTextGOAnim.SetTrigger ("playtext");
					} else if (player1counter == player2counter) {
						audioChangePhase.PlayOneShot(phaseClip);
						PHASETEXT.text = "DRAW!";
						_PhaseBGAnim.SetTrigger ("play");
						_PhaseTextGOAnim.SetTrigger ("playtext");
					}

				}
			}
		} else if (whosPlaying == 2) {
			if (player2counter <= 1 || RedUnitNumberCheck () == false) {				// Red has only 1 region left AND all regions only have 1 unit
				_blueUnitControllerScript.Deploy_RemainingUnits = REINFORCEMENTS; 		// Reinforce 5 Units
				Turns--;																// Decrement Turns Left
				whosPlaying = 1;														// Set to Blue Player Turn
				CircleOut.GetComponent<SpriteRenderer> ().material.color = myBlueColor;	// Change BGEdges to blue
				thePhase = E_PHASE.REINFORCE;											// Set the Phase to REINFORCE (so that BLUE could reinforce)
				audioChangePhase.PlayOneShot(phaseClip);
				PHASETEXT.text = thePhase.ToString ();									// Change Text accrd to thePhase
				_PhaseBGAnim.SetTrigger ("play");										// Trigger Animation
				_PhaseTextGOAnim.SetTrigger ("playtext");								// Trigger Animation

				Debug.Log("red count <= 1 && unitcheck false");
			
			} else if (player2counter > 1 && RedUnitNumberCheck () == true) {			// Red has more than 1 region AND there are region/s with more than 1 unit
				if (EndOfGame == false) {												// Check if it EndOfGame; IF NO,
					thePhase = E_PHASE.FORTIFY;											// Set the Phase to FORTIFY (so that BLUE could fortify)
					audioChangePhase.PlayOneShot(phaseClip);
					PHASETEXT.text = thePhase.ToString ();								// Change Text accrd to thePhase
					_PhaseBGAnim.SetTrigger ("play");									// Trigger Animation
					_PhaseTextGOAnim.SetTrigger ("playtext");							// Trigger Animation
					Debug.Log("red counter > 1 && unitcheck true , eog false");
				} else if (EndOfGame == true) {											// IF YES,
					if (player1counter > player2counter) {								// Change Text to who wins accordingly
						audioChangePhase.PlayOneShot(phaseClip);
						PHASETEXT.text = "BLUE WINS!";
						_PhaseBGAnim.SetTrigger ("play");
						_PhaseTextGOAnim.SetTrigger ("playtext");
					} else if (player1counter < player2counter) {
						audioChangePhase.PlayOneShot(phaseClip);
						PHASETEXT.text = "RED WINS!";
						_PhaseBGAnim.SetTrigger ("play");
						_PhaseTextGOAnim.SetTrigger ("playtext");
					} else if (player1counter == player2counter) {
						audioChangePhase.PlayOneShot(phaseClip);
						PHASETEXT.text = "DRAW!";
						_PhaseBGAnim.SetTrigger ("play");
						_PhaseTextGOAnim.SetTrigger ("playtext");
					}
				}
			}
		}
	}

	void SkipTurn(){
		switch(whosPlaying){
		case 1:
			_redUnitControllerScript.Deploy_RemainingUnits = REINFORCEMENTS;		// Reinforce 5 Units
			whosPlaying = 2; 														// Set to Red Player Turn
			CircleOut.GetComponent<SpriteRenderer> ().material.color = myRedColor; 	// Change the BGEdges to red
			thePhase = E_PHASE.REINFORCE; 											// Set the Phase to REINFORCE (so that RED could reinforce)
			audioChangePhase.PlayOneShot(phaseClip);
			PHASETEXT.text = thePhase.ToString ();									// Change Text accrd to thePhase
			_PhaseBGAnim.SetTrigger ("play"); 										// Trigger Animation
			_PhaseTextGOAnim.SetTrigger ("playtext"); 								// Trigger Animation
			break;

		case 2:
			_blueUnitControllerScript.Deploy_RemainingUnits = REINFORCEMENTS; 		// Reinforce 5 Units
			Turns--;																// Decrement Turns Left
			whosPlaying = 1;														// Set to Blue Player Turn
			CircleOut.GetComponent<SpriteRenderer> ().material.color = myBlueColor;	// Change BGEdges to blue
			thePhase = E_PHASE.REINFORCE;											// Set the Phase to REINFORCE (so that BLUE could reinforce)
			audioChangePhase.PlayOneShot(phaseClip);
			PHASETEXT.text = thePhase.ToString ();									// Change Text accrd to thePhase
			_PhaseBGAnim.SetTrigger ("play");										// Trigger Animation
			_PhaseTextGOAnim.SetTrigger ("playtext");								// Trigger Animation
			break;
		}
	}

//	void TestEndGame(){
//		EndOfGame = true;
//		if (player1counter > player2counter) {								// Change Text to who wins accordingly
//			PHASETEXT.text = "BLUE WINS!";
//			_PhaseBGAnim.SetTrigger ("play");
//			_PhaseTextGOAnim.SetTrigger ("playtext");
//		} else if (player1counter < player2counter) {
//			PHASETEXT.text = "RED WINS!";
//			_PhaseBGAnim.SetTrigger ("play");
//			_PhaseTextGOAnim.SetTrigger ("playtext");
//		} else if (player1counter == player2counter) {
//			PHASETEXT.text = "DRAW!";
//			_PhaseBGAnim.SetTrigger ("play");
//			_PhaseTextGOAnim.SetTrigger ("playtext");
//		}
//	}

	public void ButtonAttack(){
		// TO ATTACK
		if (thePhase == E_PHASE.ATTACK) {
			if (m_sceneController.GetComponent<SceneController> ().battleResults == 0) {
				if (m_AttackFrom != null &&
					m_AttackTo != null)
					m_AttackFrom.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().Attack ();
			}
		}
	}

	public void ButtonCancel(){
		// TO CANCEL ATTACK SELECTIONS
		if ((thePhase == E_PHASE.ATTACK || thePhase == E_PHASE.FORTIFY)) {
			if (m_sceneController.GetComponent<SceneController> ().battleResults == 0) {
				currentRegion = null;
				m_AttackFrom = null;
				m_AttackTo = null;
				foreach (GameObject zRegion in regionList) {
					zRegion.GetComponent<RegionScript> ().canMoveTo = false;
					zRegion.GetComponent<RegionScript> ().isSelected = false;
				}
				if (thePhase == E_PHASE.FORTIFY) {
					proceed = false;
					inAddMinus = false;
					m_mySlider.SetActive (false);
					SliderActive = false;
				}
			}
		}
	}

	public void ButtonSkipTurn(){
		if(firstSetDone == true){
			SkipTurn();
		}
	}

}
