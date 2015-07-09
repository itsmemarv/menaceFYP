using UnityEngine;
using System.Collections;

public class MyCam : MonoBehaviour {
	private Vector3 ResetCamera;
	private Vector3 Origin;
	private Vector3 Diference;
	private bool Drag = false;
	
	void Start () {
		ResetCamera = Camera.main.transform.position;
	}
	void LateUpdate () {

		Scroll();

		if (Input.GetMouseButton (2)) {
			Diference=(Camera.main.ScreenToWorldPoint (Input.mousePosition))- Camera.main.transform.position;
			if (Drag==false){
				Drag=true;
				Origin=Camera.main.ScreenToWorldPoint (Input.mousePosition);
			}
		} 
		else {
			Drag=false;
		}
		if (Drag==true){
			Camera.main.transform.position = Origin-Diference;
		}
		/*//RESET CAMERA TO STARTING POSITION WITH RIGHT CLICK
		if (Input.GetMouseButton (1)) {
			Camera.main.transform.position=ResetCamera;
		}*/
	}

	void Scroll ()
	{
		float mousePosX = Input.mousePosition.x;
		float mousePosY = Input.mousePosition.y;
		int scrollDistance = 5;
		float scrollSpeed = 70;
		const float orthographicSizeMin = 8f;
		const float orthographicSizeMax = 256f;
		
		/*if (mousePosX < scrollDistance)
		{
			transform.Translate(Vector3.right * -scrollSpeed *Time.deltaTime, Space.World);
		}
		if (mousePosX >= Screen.width - scrollDistance)
		{
			transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime, Space.World);
		}
		if (mousePosY < scrollDistance)
		{
			transform.Translate(transform.forward * -scrollSpeed * Time.deltaTime, Space.World);
		}
		if (mousePosY >= Screen.height - scrollDistance)
		{
			transform.Translate(transform.forward * scrollSpeed * Time.deltaTime, Space.World);
		}*/
		if (Input.GetAxis("Mouse ScrollWheel") < -0) // forward
		{
			Camera.main.orthographicSize *= 1.1f;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > -0) // back
		{
			Camera.main.orthographicSize *= 0.9f;
		}
		
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax );
	}

}
