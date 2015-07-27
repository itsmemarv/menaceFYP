using UnityEngine;
using System.Collections.Generic;

public class theUnit : MonoBehaviour {
	
	public static theUnit UnitControl;
	public int ID;
	public float posX;
	public float posY;
	
	public bool beingControlled;
	public bool endTurn;
	
	public GameObject m_theMap;
	MapScript _theMap;

	public GameObject m_sceneController;
	SceneController _sceneController;

	theUnit _theUnit;

	public int numberOfUnits;
	public int numberOfTrainableUnit;

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}
	
	// Use this for initialization
	void Start () {
		m_theMap = GameObject.FindGameObjectWithTag ("MAP");
		_theMap = m_theMap.GetComponent<MapScript> ();

		m_sceneController = GameObject.Find ("SceneController");
		_sceneController = m_sceneController.GetComponent<SceneController> ();
		
		numberOfUnits = 1;
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

	public void Attack(){
		_sceneController.Player1_Unit = gameObject;
		_sceneController.Player2_Unit = _theMap.m_AttackTo.GetComponent<RegionScript> ().unitOnRegion;
		
		_sceneController.Region_CurrentRegion = _theMap.m_AttackFrom;
		_sceneController.Region_SelectedRegion = _theMap.m_AttackTo;

		_sceneController.GameMap = m_theMap;
		_sceneController.inBattleScene = true;
		Application.LoadLevel (1);
	}
}


