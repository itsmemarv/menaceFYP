using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleScene : MonoBehaviour {


	public GameObject GrassTile;
	public List<GameObject> GrassList = new List<GameObject>();

	public GameObject ButtonTile;
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
	// Use this for initialization
	void Start () {
		GreenNumberOfBlobs = 5;
		OrangeNumberOfBlobs = 5;

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

	}
	
	// Update is called once per frame
	void Update () {
		SpawnBlob ();
		UpdateGreenBlob ();

		UpdateTimer ();
		AI_SpawnOrangeBlob ();
		UpdateOrangeBlob ();
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
		}
	}


	void InitialiseGreenBlobsPooling(){
		for(int a = 0; a < GreenNumberOfBlobs; ++a){
			GameObject greenblob = (GameObject)Instantiate(GreenBlob);
			greenblob.SetActive(false);
			GreenBlobList.Add(greenblob);
		}
	}

	void InitialiseOrangeBlobsPooling(){
		for(int a = 0; a < OrangeNumberOfBlobs; ++a){
			GameObject orangeblob = (GameObject)Instantiate(OrangeBlob);
			orangeblob.SetActive(false);
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
					if (blob.transform.position.x >= 7) {
						blob.SetActive(false);
						greenpoints += 5;
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
					if (blob.transform.position.x <= 0) {
						blob.SetActive(false);
						orangepoints += 5;
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

	void OnGUI(){
		GUI.Box(new Rect(10,10,100,20), "Green: " + greentimer.ToString("0"));
		GUI.Box(new Rect(10,30,100,20), "Orange: " + orangetimer.ToString("0"));

		GUI.Box(new Rect(110,10,100,20), "G Pts: " + greenpoints.ToString("0"));
		GUI.Box(new Rect(110,30,100,20), "O Pts: " + orangepoints.ToString("0"));
	}
}
