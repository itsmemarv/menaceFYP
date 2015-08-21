using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static class MyShuffler
{
	public static void Shuffle<T>(this IList<T> list)  
	{  
		for (int i = list.Count - 1; i > 0; i--) {
			int r = Random.Range(0, i);
			T tmp = list[i];
			list[i] = list[r];
			list[r] = tmp;
		}
	}
}

public class BattleScene : MonoBehaviour {
	
	public GameObject GrassTile;
	public List<GameObject> GrassList = new List<GameObject>();

	public GameObject ButtonTile;
	public List<GameObject> ButtonList = new List<GameObject> ();

	public GameObject GreenBlob;
	public GameObject OrangeBlob;

	public List<GameObject> GreenBlobList = new List<GameObject> ();
	public List<GameObject> OrangeBlobList = new List<GameObject>();

	public int GreenNumberOfBlobs;
	public int OrangeNumberOfBlobs;
	public float greentimer;
	public float orangetimer;
	public bool GreencanSpawn;
	public bool OrangecanSpawn;
	public int greenpoints;
	public int orangepoints;

	BlobScript _blob;

	public GameObject m_sceneController;
	SceneController _sceneController;

	public bool BattleEnd;
	//Ryan's codes
	/// <summary>
	/// Variables are stored here
	/// </summary>

	//random mass assigned
	public int giveMass;
	public int randRange;
	

	void SetUnitType (GameObject unit, int mass)
	{
		switch (mass) {
		case 1:
			float number = mass * 0.3f;
			unit.transform.localScale = new Vector3 (number, number, 0);
			unit.GetComponent<Rigidbody2D> ().mass = number;
			break;

		case 2:
			float number2 = mass * 0.3f;
			unit.transform.localScale = new Vector3 (number2, number2, 0);
			unit.GetComponent<Rigidbody2D> ().mass = number2;
			break;

		case 3:
			float number3 = mass * 0.3f;
			unit.transform.localScale = new Vector3 (number3, number3, 0);
			unit.GetComponent<Rigidbody2D> ().mass = number3;
			break;

		}
	}

	// Use this for initialization
	void Start () {
		m_sceneController = GameObject.Find ("SceneController");
		_sceneController = m_sceneController.GetComponent<SceneController> ();

		GreenNumberOfBlobs = _sceneController.Player1_Unit.GetComponent<theUnit>().numberOfUnits;
		OrangeNumberOfBlobs = _sceneController.Player2_Unit.GetComponent<theUnit>().numberOfUnits;

		greentimer = 5;
		orangetimer = 5;

		GreencanSpawn = false;
		OrangecanSpawn = false;

		greenpoints = 0;
		orangepoints = 0;

		InitialiseGrassTiles ();
		InitialiseClickPoint ();
		InitialiseGreenBlobsPooling ();
		InitialiseOrangeBlobsPooling ();
		giveMass = 1;
		randRange = 1;
		BattleEnd = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (BattleEnd == false) {
			SpawnBlob ();
			UpdateGreenBlob ();

			UpdateTimer ();
			AI_SpawnOrangeBlob ();
			UpdateOrangeBlob ();

			foreach (GameObject gBlob in GreenBlobList) {
				foreach (GameObject button in ButtonList) {
					if (gBlob.activeInHierarchy) {
						IgnoreCollider (button.GetComponent<Collider2D> (), gBlob.GetComponent<Collider2D> ());
					}
				}
			}

			foreach (GameObject oBlob in OrangeBlobList) {
				foreach (GameObject button in ButtonList) {
					if (oBlob.activeInHierarchy) {
						IgnoreCollider (button.GetComponent<Collider2D> (), oBlob.GetComponent<Collider2D> ());
					}
				}
			}

			if (Input.GetKeyDown ("r")) {
				MyShuffler.Shuffle (GrassList);
			}

			EndCheck();
		}
	}

	void InitialiseGrassTiles(){

		for (int x = 0; x < 8; ++x) {
			for(int y = 0; y < 3; ++y){
				GameObject m_Grass = (GameObject)Instantiate(GrassTile, new Vector3(x,y),Quaternion.identity);
				GrassList.Add(m_Grass);
			}
		}
	}

	void InitialiseClickPoint(){
		for(int y = 0; y < 3; ++y){
			GameObject m_Button = (GameObject)Instantiate(ButtonTile, new Vector3(-1,y),Quaternion.identity);
			ButtonList.Add(m_Button);
		}
	}


	void InitialiseGreenBlobsPooling(){
		for(int a = 0; a < GreenNumberOfBlobs; ++a){
			GameObject greenblob = (GameObject)Instantiate(GreenBlob);
			greenblob.SetActive(false);
			//If # of units >= 5, chance to get a bigger unit
			if(GreenNumberOfBlobs >= 5){
				randRange = Random.Range(1,100);
				if(randRange > 50){
					giveMass = randRange;
					SetUnitType(greenblob, 2);
				}
				else if(randRange < 50){
					SetUnitType(greenblob, 1);
				}
			}
			//If # of units >= 10, chance to get a biggest unit
			else if(GreenNumberOfBlobs >= 10){
				randRange = Random.Range(1,100);
				if(randRange < 25){
					giveMass = randRange;
					SetUnitType(greenblob, 2);
				}
				else if(randRange < 25){
					giveMass = randRange;
					SetUnitType(greenblob, 3);
				}
				else if(randRange > 50){
					SetUnitType(greenblob, 1);
				}
			}
			//Normal sized unit
			else {
				SetUnitType(greenblob, 1);
			}
			GreenBlobList.Add(greenblob);
		}
	}

	void InitialiseOrangeBlobsPooling(){
		for(int a = 0; a < OrangeNumberOfBlobs; ++a){
			GameObject orangeblob = (GameObject)Instantiate(OrangeBlob);
			orangeblob.SetActive(false);
			//If # of units >= 5, chance to get a bigger unit
			if(OrangeNumberOfBlobs >= 5){
				randRange = Random.Range(1,100);
				if(randRange > 50){
					giveMass = randRange;
					SetUnitType(orangeblob, 2);
				}
				else if(randRange < 50){
					SetUnitType(orangeblob, 1);
				}
			}
			//If # of units >= 10, chance to get a biggest unit
			else if(OrangeNumberOfBlobs >= 10){
				randRange = Random.Range(1,100);
				if(randRange < 25){
					giveMass = randRange;
					SetUnitType(orangeblob, 3);
				}
				else if(randRange > 25){
					giveMass = randRange;
					SetUnitType(orangeblob, 2);
				}
				else if(randRange > 50){
					SetUnitType(orangeblob, 1);
				}
			}
			//Normal sized unit
			else {
				SetUnitType(orangeblob, 1);
			}
			OrangeBlobList.Add(orangeblob);
		}
		
	}

	void SpawnBlob(){
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray, Mathf.Infinity);
			if (hitInfo) {
				if(hitInfo.transform.gameObject.tag == "Button"){
					Debug.Log("Hit");
					if (GreencanSpawn == true) {
						for(int p = 0; p < GreenBlobList.Count; ++p) {
							if(!GreenBlobList[p].activeInHierarchy){
								GreenBlobList[p].transform.position = new Vector3(hitInfo.transform.position.x,hitInfo.transform.position.y,-2);
								GreenBlobList[p].SetActive(true);
								GreencanSpawn = false;
								greentimer = 5;
								break;
							}
						}
					}
				}
			}
		}
	}

	void UpdateGreenBlob(){
		if (GreenBlobList != null) {
			foreach (GameObject blob in GreenBlobList) {
				if(blob.activeInHierarchy){
					_blob = blob.GetComponent<BlobScript>();
					_blob.currPos = blob.transform.position;
					if (_blob.currPos != _blob.lastPos) {
						
						_blob.x = _blob.currPos.x - _blob.lastPos.x;
						Debug.Log("R-T =: " + _blob.x);
					}
					_blob.lastPos = _blob.currPos;
					
					if (blob.transform.position.x >= 7) {
						blob.SetActive(false);
						//greenpoints += 5;
					}
					else if (blob.transform.position.x < -1 && _blob.x < 0){
						GreenBlobList.Remove(blob);
						Destroy(blob);
					}
					else{
						blob.transform.Translate(Vector3.right * Time.deltaTime);
					}
				}
			}
		}
	}

	void UpdateOrangeBlob(){
		if (GreenBlobList != null) {
			foreach (GameObject blob in OrangeBlobList) {
				if(blob.activeInHierarchy){
					_blob = blob.GetComponent<BlobScript>();
					_blob.currPos = blob.transform.position;
					if (_blob.currPos != _blob.lastPos) {
						
						_blob.x = _blob.currPos.x - _blob.lastPos.x;
						Debug.Log("R-T =: " + _blob.x);
					}
					_blob.lastPos = _blob.currPos;

					if (blob.transform.position.x <= -1) {
						blob.SetActive(false);
						//orangepoints += 5;
					}
					else if (blob.transform.position.x > 7 && _blob.x > 0){
						OrangeBlobList.Remove(blob);
						Destroy(blob);
					}
					else{
						blob.transform.Translate(-Vector3.right * Time.deltaTime);
					}
				}
			}
		}
	}

	void UpdateTimer(){
		if (greentimer <= 0) {
			GreencanSpawn = true;
		} else {
			greentimer -= Time.deltaTime;
		}

		if (orangetimer <= 0) {
			OrangecanSpawn = true;
		} else {
			orangetimer -= Time.deltaTime;
		}
	}

	void AI_SpawnOrangeBlob(){
		if (OrangecanSpawn == true) {
			for(int o = 0; o < OrangeBlobList.Count; ++o) {
				if(!OrangeBlobList[o].activeInHierarchy){
					OrangeBlobList[o].transform.position = new Vector3(7,Random.Range(0,3),-2);
					OrangeBlobList[o].SetActive(true);
					OrangecanSpawn = false;
					orangetimer = 5;
					Debug.Log("Spawned");
					break;
				}
			}
		}
	}

	void IgnoreCollider(Collider2D button, Collider2D sprite){
		Physics2D.IgnoreCollision (button, sprite);
	}

	void EndCheck(){
		if (OrangeBlobList.Count <= 0 || GreenBlobList.Count <= 0) {
			if(OrangeBlobList.Count <= 0){
				_sceneController.Player1_Unit.GetComponent<theUnit>().numberOfUnits = GreenBlobList.Count;
				Debug.Log("# of Player Remaining Units: " + GreenBlobList.Count);
				_sceneController.battleResults = 1;
				
				System.Threading.Thread.Sleep(1000);
				_sceneController.Player1_Unit = null;
				_sceneController.Player2_Unit = null;
				_sceneController.Region_SelectedRegion = null;
				_sceneController.Region_CurrentRegion = null;
				_sceneController.GameMap = null;
				_sceneController.inBattleScene = false;
				Application.LoadLevel("GameScene");
				BattleEnd = true;
			}

			else if (GreenBlobList.Count <= 0){
				_sceneController.Player2_Unit.GetComponent<theUnit>().numberOfUnits = OrangeBlobList.Count;
				_sceneController.Player1_Unit.GetComponent<theUnit>().numberOfUnits = 1;
				Debug.Log("# of Enemy Remaining Units: " + OrangeBlobList.Count);
				Debug.Log("# of Player Remaining Units: " + _sceneController.Player1_Unit.GetComponent<theUnit>().numberOfUnits);
				_sceneController.battleResults = 2;
				
				System.Threading.Thread.Sleep(1000);
				_sceneController.Player1_Unit = null;
				_sceneController.Player2_Unit = null;
				_sceneController.Region_SelectedRegion = null;
				_sceneController.Region_CurrentRegion = null;
				_sceneController.GameMap = null;
				_sceneController.inBattleScene = false;
				Application.LoadLevel("GameScene");
				BattleEnd = true;
			}
		}
	}
	void OnGUI(){
		GUI.Box(new Rect(10,10,100,20), "Green: " + greentimer.ToString("0"));
		GUI.Box(new Rect(10,30,100,20), "Orange: " + orangetimer.ToString("0"));

		GUI.Box(new Rect(110,10,100,20), "G Remaining: " + GreenBlobList.Count.ToString("0"));
		GUI.Box(new Rect(110,30,100,20), "O Remaining: " + OrangeBlobList.Count.ToString("0"));
	}
}
