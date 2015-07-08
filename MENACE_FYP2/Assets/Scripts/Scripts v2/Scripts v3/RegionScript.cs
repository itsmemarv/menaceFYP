using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RegionScript : MonoBehaviour {

	public static RegionScript RegionControl;
	//GameObject theMap;
	public int region_Owner;
	public int region_ID;

	public bool isSelected;
	public Color origColor;

	public bool canMoveTo;
	public bool hasUnit;

	public float regionX;
	public float regionY;

	public GameObject unitOnRegion;
	// Use this for initialization

	void Awake(){
		//if (RegionControl == null) {
			//DontDestroyOnLoad (gameObject);
			//RegionControl = this;
		//} else if(RegionControl != this){
		//	Destroy(gameObject);
		//}
	}
	void Start () {
		//theMap = GameObject.FindGameObjectWithTag ("MAP");
		regionX = gameObject.transform.position.x;
		regionY = gameObject.transform.position.y;
		origColor = gameObject.GetComponent<SpriteRenderer> ().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (isSelected == true) {
			gameObject.GetComponent<SpriteRenderer> ().material.color = Color.grey;
		} else {
			gameObject.GetComponent<SpriteRenderer> ().material.color = origColor;
		}

		if (canMoveTo == true) {
			gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
			if (isSelected == true) {
				gameObject.GetComponent<SpriteRenderer> ().material.color = Color.grey;
			}
				else{
				gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
				}
		}
	}

	// Shows a text who is the owner
	void OnGUI(){
		Vector3 p = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		GUI.Label (new Rect (p.x - 10,Screen.height-(p.y + 20), 50, 20), region_Owner.ToString());
	}

}
