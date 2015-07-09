using UnityEngine;
using System.Collections;

public class UnitMove : MonoBehaviour {
	//public GameObject a;
	private bool flag = false;
	//public TileMouseOver unitSelect;
	UnitSelect select;


	private Vector3 endPoint;
	private float yAxis;
	public float duration = 10.0f;

	// Use this for initialization
	void Start () {
		yAxis = gameObject.transform.position.y;
		//Select.unitSelect = false;
		//Select = gameObject.GetComponent<TileMouseOver>().unitSelect;

	}
	
	// Update is called once per frame
	void Update () {
		//if (select.isSelected == true)
		//if (GetComponent<UnitSelect>().isSelected == true)
		//{
			if ((/*unitSelect.unitSelect == true &&*/ Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(1)))
			{
				RaycastHit hit;
				Ray ray;
	#if UNITY_EDITOR
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
				ray = Camera.mainScreenPointToRay(Input.GetTouch(0).Position);
	#endif
				if(Physics.Raycast(ray,out hit))
				{
					flag = true;
					endPoint = hit.point;
					endPoint.y = yAxis;
					Debug.Log(endPoint);
				}
			}
			if (flag && !Mathf.Approximately(gameObject.transform.position.magnitude, endPoint.magnitude))
			{
				gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, endPoint, 1/(duration*(Vector3.Distance(gameObject.transform.position, endPoint))));
			}
			else if (flag && Mathf.Approximately(gameObject.transform.position.magnitude, endPoint.magnitude))
			{
				flag = false;
				Debug.Log("I am here");
			}
		//}
	}
}
