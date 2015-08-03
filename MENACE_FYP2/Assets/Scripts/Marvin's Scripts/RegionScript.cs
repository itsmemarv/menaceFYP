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

//		if (region_Owner == 1) {
//			gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(0.173f, 0.157f, 0.639f));
//		}
//		else if (region_Owner == 2) {
//			gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(0.639f, 0.173f, 0.157f));
//		}

//		if (isSelected == true) {
//			gameObject.GetComponent<SpriteRenderer> ().material.color = Color.grey;
//		} else if (isSelected == false){
//			gameObject.GetComponent<SpriteRenderer> ().material.color = origColor;
//		}

		gameObject.GetComponent<SpriteRenderer> ().material.color = origColor;

		if (isSelected == true) {
			gameObject.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(0.0f,0.0f,0.0f,0.5f));
		}

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

}
