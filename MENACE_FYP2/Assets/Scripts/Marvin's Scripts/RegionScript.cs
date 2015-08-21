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

	public int ResourceValue;

	private GameObject m_child;
	private TextMesh text_child;
	// Use this for initialization
	void Start () {
		//theMap = GameObject.FindGameObjectWithTag ("MAP");
		regionX = gameObject.transform.position.x;
		regionY = gameObject.transform.position.y;
		origColor = gameObject.GetComponent<SpriteRenderer> ().material.color;

		m_child = gameObject.transform.GetChild(0).gameObject;
		text_child = m_child.GetComponent<TextMesh> ();

		SetResourceValue (Random.Range (1, 6));
	}
	
	// Update is called once per frame
	void Update () {
	
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

	void SetResourceValue(int randNum){
		switch (randNum) {
		case 1:
			ResourceValue = 100;
			text_child.text = ResourceValue.ToString ();
			break;
		case 2:
			ResourceValue = 200;
			text_child.text = ResourceValue.ToString ();
			break;
		case 3:
			ResourceValue = 300;
			text_child.text = ResourceValue.ToString ();
			break;
		case 4:
			ResourceValue = 400;
			text_child.text = ResourceValue.ToString ();
			break;
		case 5:
			ResourceValue = 500;
			text_child.text = ResourceValue.ToString ();
			break;
		}

	}

}
