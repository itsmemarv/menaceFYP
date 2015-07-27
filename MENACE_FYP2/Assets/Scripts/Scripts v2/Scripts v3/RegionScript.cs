using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RegionScript : MonoBehaviour {

	public static RegionScript RegionControl;
	public int region_Owner;

	public bool isSelected;
	public bool canMoveTo;
	public bool hasUnit;

	public float regionX;
	public float regionY;

	public GameObject unitOnRegion;
	public Color origColor;


	// Use this for initialization
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
		} else if (isSelected == false){
			gameObject.GetComponent<SpriteRenderer> ().material.color = origColor;
		}
		//gameObject.GetComponent<SpriteRenderer> ().material.color = origColor;
		if (canMoveTo == true) {
			gameObject.GetComponent<SpriteRenderer> ().material.color = Color.black;
			if (isSelected == true) {
				//gameObject.GetComponent<SpriteRenderer> ().material.color = Color.grey;
				gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(0.0f,0.0f,0.0f,0.5f));
			} else {
				gameObject.GetComponent<SpriteRenderer> ().material.color = Color.black;
			}
		}
	}

	// Shows a text who is the owner
	void OnGUI(){
		Vector3 p = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		if (unitOnRegion != null) {
			GUI.Label (new Rect (p.x - 10, Screen.height - (p.y + 20), 50, 20), unitOnRegion.GetComponent<theUnit> ().numberOfUnits.ToString ());
		}
	}

}
