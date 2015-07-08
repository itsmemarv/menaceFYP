using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleColor : MonoBehaviour {

	public GameObject sceneController;

	// Use this for initialization
	void Start () {
		sceneController = GameObject.Find("SceneController");
		gameObject.GetComponent<Renderer>().material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (GUI.Button (new Rect (20, 40, 200, 20), "GO TO GAME SCENE")) {
			sceneController.GetComponent<SceneController>().Player1_Unit = null;
			sceneController.GetComponent<SceneController>().Player2_Unit = null;
			sceneController.GetComponent<SceneController>().Region_onPlay = null;
			sceneController.GetComponent<SceneController>().inBattleScene = false;
			Application.LoadLevel (0);
		}
	}


}
