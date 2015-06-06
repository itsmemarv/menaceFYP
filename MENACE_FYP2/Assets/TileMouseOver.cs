using UnityEngine;
using System.Collections;

public class TileMouseOver : MonoBehaviour {


	public Color highlightColor;
	Color normalColor;
	// Use this for initialization
	void Start () {
		normalColor = GetComponent<Renderer> ().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (gameObject.GetComponent<Collider>().Raycast (ray, out hit, Mathf.Infinity)) {
			GetComponent<Renderer> ().material.color = highlightColor;
		} else {
			GetComponent<Renderer>().material.color = normalColor;
		}
	}
}
