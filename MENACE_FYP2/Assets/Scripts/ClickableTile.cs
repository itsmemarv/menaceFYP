using UnityEngine;
using System.Collections;

public class ClickableTile : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TileMap map;
	public int TILEOWNER;
	public bool containUnit;


	void Start(){
	}
	void Update(){
		if(TILEOWNER == 2){
			gameObject.GetComponent<Renderer>().material.color = Color.red;
			gameObject.tag = "Enemy";
		}
		else if(TILEOWNER == 1){
			gameObject.GetComponent<Renderer>().material.color = Color.blue;
			gameObject.tag = "Player";
		}
		else if(TILEOWNER == 0){
			gameObject.GetComponent<Renderer>().material.color = Color.white;
			gameObject.tag = "Neutral";
		}
	}

	void OnMouseDown() {

		Debug.Log ("Click!");
		map.GeneratePathTo (tileX, tileY);

	}


	
	
	
}
