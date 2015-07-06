using UnityEngine;
using System.Collections;

public class DragRender : MonoBehaviour {
	/*void OnEnable() {
		Gesture.onDraggingE += OnDrag;
	}
	
	void OnDisable() {
		Gesture.onDraggingE -= OnDrag;
	}
	
	void OnDrag( DragInfo drag ) {
		Vector3 pos = Camera.main.ScreenToWorldPoint( drag.pos );
		pos.z = 0;
		transform.position = pos;
	}*/

	public float smooth = 100.0f;
	public float speed = 0.5f;

	void Start ()
	{

	}
	void Update ()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
			{
				Vector2 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
				transform.position = Vector3.Lerp(transform.position, touchPosition, Time.deltaTime);
			}
		}
	}
}