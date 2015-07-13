﻿using UnityEngine;
using System.Collections.Generic;

public class theUnit : MonoBehaviour {
	
	public static theUnit UnitControl;
	public int ID;
	public float posX;
	public float posY;
	
	public bool beingControlled;
	public bool endTurn;
	
	public GameObject theMap;
	public GameObject sceneController;
	
	public int numberOfUnits;
	public int numberOfTrainableUnit;

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}
	
	// Use this for initialization
	void Start () {
		theMap = GameObject.FindGameObjectWithTag ("MAP");
		sceneController = GameObject.Find ("SceneController");
		
		numberOfUnits = 5;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateUnitColors ();
		
	}
	
	void UpdateUnitColors(){
		if(gameObject.tag == "unit_Player1"){
			gameObject.GetComponent<Renderer>().material.color = Color.blue;
		}
		else if(gameObject.tag == "unit_Player2"){
			gameObject.GetComponent<Renderer>().material.color = Color.red;
		}
	}
	
	public bool MoveToSelectedTile(){
		if (theMap.GetComponent<MapScript> ().selectedRegion != null) {
			if (theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().canMoveTo == true) {
				if (theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().hasUnit == false) {
					Debug.Log ("HAS NO UNIT");
					//					theMap.GetComponent<MapScript> ().previousRegion = theMap.GetComponent<MapScript> ().currentRegion; 														// Set Previous Tile
					
					//					gameObject.transform.position = theMap.GetComponent<MapScript> ().selectedRegion.transform.position;																						// Move the Unit
					//					gameObject.GetComponent<theUnit> ().posX = (float)gameObject.transform.position.x;
					//					gameObject.GetComponent<theUnit> ().posY = (float)gameObject.transform.position.y;
					
					
					GameObject newUnit = (GameObject)Instantiate (theMap.GetComponent<MapScript> ().Unit,
					                                              theMap.GetComponent<MapScript> ().selectedRegion.transform.position,
					                                              Quaternion.identity); 																		// Create a New Unit on previous tile
					
					
					newUnit.GetComponent<theUnit> ().posX = (float)theMap.GetComponent<MapScript> ().selectedRegion.transform.position.x;
					newUnit.GetComponent<theUnit> ().posY = (float)theMap.GetComponent<MapScript> ().selectedRegion.transform.position.y;
					newUnit.GetComponent<theUnit> ().ID = theMap.GetComponent<MapScript> ().selectedUnit.GetComponent<theUnit> ().ID;
					if(newUnit.GetComponent<theUnit> ().ID == 1)
					{
						newUnit.GetComponent<theUnit>().numberOfTrainableUnit = (int) (2 * theMap.GetComponent<MapScript> ().player1counter) + 1;
						theMap.GetComponent<MapScript> ().player1counter ++;
						theMap.GetComponent<MapScript>().currentRegion.GetComponent<RegionScript>().unitOnRegion.GetComponent<theUnit>().numberOfTrainableUnit = (int) (2 * theMap.GetComponent<MapScript> ().player1counter) + 1;
					} else if (newUnit.GetComponent<theUnit> ().ID == 2){
						newUnit.GetComponent<theUnit>().numberOfTrainableUnit = (int) (2 * theMap.GetComponent<MapScript> ().player2counter) + 1;
						theMap.GetComponent<MapScript> ().player2counter ++;
						theMap.GetComponent<MapScript>().currentRegion.GetComponent<RegionScript>().unitOnRegion.GetComponent<theUnit>().numberOfTrainableUnit = (int) (2 * theMap.GetComponent<MapScript> ().player2counter) + 1;
					}
					newUnit.GetComponent<theUnit> ().tag = theMap.GetComponent<MapScript> ().selectedUnit.GetComponent<theUnit> ().tag;//"unit_Player2";
					newUnit.transform.parent = theMap.GetComponent<MapScript> ().controller.transform;
					
					theMap.GetComponent<MapScript> ().controller.GetComponent<SceneController> ().objectsInHierarchy.Add (newUnit);
					theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion = newUnit;
					theMap.GetComponent<MapScript> ().unitList.Add (newUnit);
					
					theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().region_Owner = gameObject.GetComponent<theUnit> ().ID; 						// Change Tile ID according to who is playing

					theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().hasUnit = true;									 						 
					

					return true;
					
				} else if (theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().hasUnit == true && // Tile has own Unit
				           theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().region_Owner == gameObject.GetComponent<theUnit> ().ID) {
					Debug.Log ("HAS UNIT && SAME OWNER");
					//gameObject.transform.position = theMap.GetComponent<MapScript> ().selectedRegion.transform.position;

					return true;
					
				}
//					else if (theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().hasUnit == true && // Tile has own Unit
//				           theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().region_Owner != gameObject.GetComponent<theUnit> ().ID) {
//					Debug.Log ("HAS UNIT && DIFFERENT OWNER!! ATTACK!!");
//										sceneController.GetComponent<SceneController>().Player1_Unit = gameObject;
//										sceneController.GetComponent<SceneController>().Player2_Unit = theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript>().unitOnRegion;
//										sceneController.GetComponent<SceneController>().Region_onPlay = theMap.GetComponent<MapScript> ().selectedRegion;
//										sceneController.GetComponent<SceneController>().inBattleScene = true;
//										Application.LoadLevel(1);
//					
////					int rng;
////					rng =  Random.Range (1, 100);
////					Debug.Log ("RNG: " +rng);
////					// P1 == P2
////					if (gameObject.GetComponent<theUnit> ().numberOfUnits == theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits) {
////						if (rng <= 50) { //P1 WIN
////							theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
////							theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
////							theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
////							gameObject.GetComponent<theUnit> ().numberOfUnits = 1;
////						} else if (rng > 50) { //P2 WIN
////							theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
////							theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
////							theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
////							gameObject.GetComponent<theUnit> ().numberOfUnits = 1;
////						}
////					}
////					
////					// P1 > P2
////					if (gameObject.GetComponent<theUnit> ().numberOfUnits > theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits) {
////						theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
////						theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
////						theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
////						gameObject.GetComponent<theUnit> ().numberOfUnits = 1;
////					}
////					
////					// P1 < P2
////					if (gameObject.GetComponent<theUnit> ().numberOfUnits < theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits) {
////						theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
////						theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
////						theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
////						gameObject.GetComponent<theUnit> ().numberOfUnits = 1;
////					}
//					
//					
//					
//					return true;
//					
//				} 
					else {
					return false;
				}
			} else {
				return false;
			}
		} else {
			Debug.Log ("no selected tile, cannot move");
			return false;
		}
		
	}

	public bool AttackSelectedTile(){
		if (theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().hasUnit == true && // Tile has own Unit
			theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().region_Owner != gameObject.GetComponent<theUnit> ().ID) {
			Debug.Log ("HAS UNIT && DIFFERENT OWNER!! ATTACK!!");
			sceneController.GetComponent<SceneController> ().Player1_Unit = gameObject;
			sceneController.GetComponent<SceneController> ().Player2_Unit = theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion;
			sceneController.GetComponent<SceneController> ().Region_SelectedRegion = theMap.GetComponent<MapScript> ().selectedRegion;
			sceneController.GetComponent<SceneController> ().Region_CurrentRegion = theMap.GetComponent<MapScript> ().currentRegion;
			sceneController.GetComponent<SceneController> ().GameMap = theMap;
			sceneController.GetComponent<SceneController> ().inBattleScene = true;
			Application.LoadLevel (1);
			
			//int rng;
//			rng =  Random.Range (1, 100);
//			Debug.Log ("RNG: " +rng);
//			// P1 == P2
//			if (gameObject.GetComponent<theUnit> ().numberOfUnits == theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits) {
//				if (rng <= 50) { //P1 WIN
//					theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
//					theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
//					theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
//					gameObject.GetComponent<theUnit> ().numberOfUnits = 1;
//					if(theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 1){
//						theMap.GetComponent<MapScript> ().player1counter ++;
//						theMap.GetComponent<MapScript> ().player2counter --;
//					}
//					else if (theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 2){
//						theMap.GetComponent<MapScript> ().player2counter ++;
//						theMap.GetComponent<MapScript> ().player1counter --;
//					}
//
//				} else if (rng > 50) { //P2 WIN
//					theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
//					theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
//					theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
//					gameObject.GetComponent<theUnit> ().numberOfUnits = 1;
//					if(theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 1){
//						theMap.GetComponent<MapScript> ().player1counter ++;
//						theMap.GetComponent<MapScript> ().player2counter --;
//					}
//					else if (theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 2){
//						theMap.GetComponent<MapScript> ().player2counter ++;
//						theMap.GetComponent<MapScript> ().player1counter --;
//					}
//
//				}
//			}
//			
//			// P1 > P2
//			else if (gameObject.GetComponent<theUnit> ().numberOfUnits > theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits) {
//				theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
//				theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
//				theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
//				gameObject.GetComponent<theUnit> ().numberOfUnits = 1;
//				if(theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 1){
//					theMap.GetComponent<MapScript> ().player1counter ++;
//					theMap.GetComponent<MapScript> ().player2counter --;
//				}
//				else if (theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 2){
//					theMap.GetComponent<MapScript> ().player2counter ++;
//					theMap.GetComponent<MapScript> ().player1counter --;
//				}
//			}
//			
//			// P1 < P2
//			else if (gameObject.GetComponent<theUnit> ().numberOfUnits < theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits) {
//				theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
//				theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = theMap.GetComponent<MapScript> ().selectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
//				theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
//				gameObject.GetComponent<theUnit> ().numberOfUnits = 1;
//				if(theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 1){
//					theMap.GetComponent<MapScript> ().player1counter ++;
//					theMap.GetComponent<MapScript> ().player2counter --;
//				}
//				else if (theMap.GetComponent<MapScript> ().currentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 2){
//					theMap.GetComponent<MapScript> ().player2counter ++;
//					theMap.GetComponent<MapScript> ().player1counter --;
//				}
//			}
			
			return true;

			
		} else {
			return false;
		}
	}

	public void TrainUnit(){
		if (numberOfTrainableUnit > 0) {
			gameObject.GetComponent<theUnit> ().numberOfUnits ++;
			numberOfTrainableUnit --;
			Debug.Log ("trainable unit" + numberOfTrainableUnit);
		}
	}
}


