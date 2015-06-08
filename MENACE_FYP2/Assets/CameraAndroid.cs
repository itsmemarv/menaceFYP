using UnityEngine;
using System.Collections;

public class CameraAndroid : MonoBehaviour {

	public float PanSpeed = 0.025F;
	public float PinchSpeed = 0.05F;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			// Translate along world cordinates. (Done this way so we can angle the camera freely.)
			transform.position -= new Vector3(touchDeltaPosition.x * PanSpeed, 0, touchDeltaPosition.y * PanSpeed);
		}
		if ( Input.touchCount == 2 )
		{
			Touch touch1 = Input.GetTouch( 0 );
			Touch touch2 = Input.GetTouch( 1 );
			
			// Find out how the touches have moved relative to eachother.
			Vector2 curDist = touch1.position - touch2.position;
			Vector2 prevDist = (touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition);
			
			float touchDelta = curDist.magnitude - prevDist.magnitude;
			
			// Translate along local coordinate space.
			Camera.main.transform.Translate(0, 0, touchDelta * PinchSpeed);   
		}
	}
}
