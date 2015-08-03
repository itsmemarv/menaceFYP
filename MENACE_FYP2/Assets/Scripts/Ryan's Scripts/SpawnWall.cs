using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnWall : MonoBehaviour {

	public List<GameObject> _list;
	public GameObject wallItem;
	//GameObject spawn = GameObject.FindGameObjectWithTag("SpawnPoint");
	public int wallLength = 20;
	public float allowance = 1.0f;
	public float spawnSpeed = 0.0f;

	// Use this for initialization
	void Start () {
		_list = new List<GameObject>();
		for(int x = 0; x < wallLength; x++){
			for(int z = 0; z < 1; z++){
				Vector3 pos = new Vector3(x, 0.5f, z) * allowance;
				Vector3 actual = new Vector3(5.5f,0,10);
				_list.Add ((GameObject)Instantiate(wallItem, pos + actual, Quaternion.identity));
			}
		}

	}
	
	// Update is called once per frame
	void Update () {

	}
}
