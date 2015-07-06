using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]

public class CameraController : MonoBehaviour {

	Vector3 hit_position = Vector3.zero;
	Vector3 current_position = Vector3.zero;
	Vector3 camera_position = Vector3.zero;

	public float minDist = 2.0f;
	public float maxDist = 5.0f;
	public Transform projectile;
	public float speed;
	private Vector3 moveVec;
	private float startZ;
	private float actualDist;
	private Vector2 dragStartPos;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.Landscape;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetMouseButtonDown(1))
		{
			hit_position = Input.mousePosition;
			camera_position = transform.position;
		}
		else if(Input.GetMouseButton(2))
		{

		}
		if(Input.GetMouseButton (1))
		{
			current_position = Input.mousePosition;
			LeftMouseDrag();
		}*/

		if (Input.touchCount == 1 ) 
		{                
			Touch touch = Input.GetTouch(0);
			switch (touch.phase) 
			{
			case TouchPhase.Began:
				dragStartPos = touch.position;
				moveVec = Vector2.zero;
				break;
				
			case TouchPhase.Moved:
				Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
				pos.z = startZ;
				projectile.position = Camera.main.ScreenToWorldPoint(touch.position);
				//here i gave condition to move camera with in required position 
				if(projectile.position.z >= -150 && projectile.position.z <= 100)
				{
					moveVec = -(touch.position - dragStartPos) * speed;
				}
				break;
				
			case TouchPhase.Ended:
				dragStartPos = touch.position;
				moveVec = Vector2.zero;
				break;
			}
			projectile.Translate(moveVec * Time.deltaTime);
			Vector3 val = moveVec * Time.deltaTime;
		}

		//ZOOM
		if (Input.touchCount == 2) 
		{
			Touch touch = Input.GetTouch(0);
			Touch touch1 = Input.GetTouch(1);
			if (touch.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved) 
			{
				Vector2 curDist = touch.position - touch1.position;
				Vector2 prevDist = (touch.position - touch.deltaPosition) - (touch1.position - touch1.deltaPosition);
				float delta = curDist.magnitude - prevDist.magnitude;
				Camera.main.transform.Translate(0,0,delta * .5f);
			}
		}
	}

	//do function
	void LeftMouseDrag()
	{
		current_position.z = hit_position.z = camera_position.y;

		Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);
		
		direction = direction * -1;
		Vector3 position = camera_position + direction;
		
		transform.position = position;
	}
}
