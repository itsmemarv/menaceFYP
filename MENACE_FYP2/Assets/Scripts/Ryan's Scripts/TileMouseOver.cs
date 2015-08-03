using UnityEngine;
using System.Collections;

public class TileMouseOver : MonoBehaviour {

	GameObject unit;
	public bool unitSelect = false;
	public float speed = 0.03f;

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
					unitSelect = true;
				}



			}
			else if (t.tapCount == 1)
			{
				GetComponent<Renderer>().material.color = normalColor;
				unitSelect = false;
			}
			//if ()
			//else if touch drag
			//unit move
			//double tap ON normal ground and drag == marquee for selecting units when let go.
			//double taps = selections
			//single taps = cancel or deselect
			//single tap n drag does nothing
			//double tap ON units and drag to move
			if (unitSelect == true)
			{
				if (Input.touchCount > 0) {
					Touch touch = Input.GetTouch (0);
					if (touch.phase == TouchPhase.Began && touch.phase == TouchPhase.Moved)
					{
						Plane plane = new Plane(Vector3.up, transform.position);
						float dist;
						Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
						if(plane.Raycast (myRay, out dist))
						{
							transform.position = Vector3.MoveTowards(transform.position, myRay.GetPoint(dist), Time.deltaTime * speed);
							Debug.Log("Running");
						}
					}
				}
			}
		}
	}
}
