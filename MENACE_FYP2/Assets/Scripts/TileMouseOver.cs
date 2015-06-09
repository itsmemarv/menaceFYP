using UnityEngine;
using System.Collections;

public class TileMouseOver : MonoBehaviour {

	GameObject unit;

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

		foreach(Touch t in Input.touches)
		{
			if (t.tapCount == 2)
			{
				if (gameObject.GetComponent<Collider>().Raycast (ray, out hit, Mathf.Infinity)) {
					GetComponent<Renderer> ().material.color = highlightColor;
				}

			}
			else if (t.tapCount == 1)
			{
				GetComponent<Renderer>().material.color = normalColor;
			}
			if ()
			//else if touch drag
			//unit move
			//double tap ON normal ground and drag == marquee for selecting units when let go.
			//double taps = selections
			//single taps = cancel or deselect
			//single tap n drag does nothing
			//double tap ON units and drag to move
		}
	}
}
