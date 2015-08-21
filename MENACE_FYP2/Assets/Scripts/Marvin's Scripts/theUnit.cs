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

	private GameObject m_child;
	private TextMesh text_child;

	private Color textRed;
	private Color textBlue;

	public bool HasMoreThan1Unit;

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
		
		m_child = gameObject.transform.GetChild(0).gameObject;
		text_child = m_child.GetComponent<TextMesh> ();

		textBlue = new Color (0.706f, 0.706f, 1.0f);
		textRed = new Color (1.0f, 0.706f, 0.706f);

		HasMoreThan1Unit = false;
	}
	
	// Update is called once per frame
	void Update () {

		HasMoreThan1UnitCheck ();
		UpdateNumberText ();
	}

	void UpdateNumberText(){
		text_child.text = numberOfUnits.ToString ();
		if(gameObject.tag == "unit_Player1"){
			text_child.color = textBlue;
		}
		else if(gameObject.tag == "unit_Player2"){
			text_child.color = textRed;
		}
	}

	public void HasMoreThan1UnitCheck(){
		if (numberOfUnits > 1) {
			HasMoreThan1Unit = true;
		} else {
			HasMoreThan1Unit = false;
		}
	}

	public void Attack(){
		_sceneController.Player1_Unit = gameObject;
		_sceneController.Player2_Unit = _theMap.m_AttackTo.GetComponent<RegionScript> ().unitOnRegion;
		
		_sceneController.Region_CurrentRegion = _theMap.m_AttackFrom;
		_sceneController.Region_SelectedRegion = _theMap.m_AttackTo;

		_sceneController.GameMap = m_theMap;
		_sceneController.inBattleScene = true;
		Application.LoadLevel("BumpScene");
	}
}


