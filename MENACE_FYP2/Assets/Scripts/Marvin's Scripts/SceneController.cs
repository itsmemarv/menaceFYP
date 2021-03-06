﻿using UnityEngine;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {

	public static SceneController sceneControl;
	public List<GameObject> objectsInHierarchy = new List<GameObject> ();


	public GameObject Player1_Unit;
	public GameObject Player2_Unit;
	public GameObject Region_SelectedRegion;
	public GameObject Region_CurrentRegion;
	public GameObject GameMap;

	public int battleResults;
	// 1 = win
	// 2 = lose
	// 3 = draw


	public bool inBattleScene;

	void Awake(){
		if (sceneControl == null) {
			DontDestroyOnLoad (gameObject);
			sceneControl = this;
		} else if(sceneControl != this){
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		inBattleScene = false;
		foreach (Transform child in transform) {
			objectsInHierarchy.Add(child.gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject obj in objectsInHierarchy) {
			obj.SetActive (true);

			if (inBattleScene) {
				obj.SetActive (false);
			}
		}
	}

	
	public void Back2Menu(string SceneName){
		Destroy (gameObject);
		Application.LoadLevel (SceneName);
	}
	
}
