using UnityEngine;
using System.Collections;

public class BlobScript : MonoBehaviour {

	public Vector3 currPos;
	public Vector3 lastPos;
	public float x;

	// Use this for initialization
	void Start () {
		currPos = transform.position;
		lastPos = Vector3.zero;
		x = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
