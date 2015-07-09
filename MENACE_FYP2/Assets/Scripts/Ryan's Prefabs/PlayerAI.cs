using UnityEngine;
using System.Collections;

public class PlayerAI : MonoBehaviour {

	GameObject Enemy;
	// Use this for initialization
	void Start () {
		Enemy = GameObject.Find("Enemy(Clone)");
	}
	
	// Update is called once per frame
	void Update () {
		if (Enemy)
			GetComponent<NavMeshAgent>().destination = Enemy.transform.position;
	}
}
