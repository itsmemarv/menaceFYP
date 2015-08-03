using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDivision : MonoBehaviour {

	public int spawn;
	public int cols;
	public int rows;
	public GameObject [][]soldier;
	public Object [] man;
	public GameObject [] manObject;

	public GameObject Soldier;
	public Formation formation;
	public enum Formation {Square, Rectamgle, Triangle}
//	GameObject[] gridArray = new GameObject[64];
//	List<GameObject> members = new List<GameObject>();
	void Start () {

	}
	// Use this for initialization
	/*void Start () {

		//Init variables
		rows = 10;
		cols = 10;
		spawn = 15;
		//spawn a soldier in a row
		soldier = new GameObject[rows][];

		man = Resources.LoadAll("Unit", typeof(GameObject));

		for (int i = 0; i < man.Length; i++)
			Debug.Log("man #" + i + "is " + man[i].name);

		for (int i = 0; i < rows; i++){
			soldier[i] = new GameObject[rows];
			for (int j = 0; j < cols; j++){
				soldier[i][j] = (GameObject) Instantiate (man[i], new Vector2(i,j), Quaternion.identity);
			}
		}
	}*/
	
	// Update is called once per frame
	void Update () {
	
	}

	void setFormation(Formation newFormation){
		switch (newFormation){
		
			case Formation.Square:
				Debug.Log ("Square");
			break;
		
			case Formation.Rectamgle:
				Debug.Log ("Rectangle");
			break;

			case Formation.Triangle:
				Debug.Log ("Triangle");
			break;
		}
		
	}

	void inf_UnitDown(string unitName){
		//loop list check if dead
		//remove
		//check current form
		//choose new form, end
	}

	void Movement(Vector3 newPos){
		//choose middle on index unit to be leader
		//all move tgt with it
		// for n % 2 != 0 squads >> pick mid membe
		//calculate dist between unit and newPos, get direction all move acc to dist and dir
	}

	/*public void OnMouseClick()
	{
		foreach(GameObject item in gridArray)
		{
			//member.Select(); // Highlight unit(s)
		}
	}*/

}
