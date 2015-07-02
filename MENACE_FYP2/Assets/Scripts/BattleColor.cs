using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleColor : MonoBehaviour {



	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer>().material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
