using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	GameObject Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Cube(Clone)");
	}
	
	// Update is called once per frame
	void Update () {
		if (Player)
			GetComponent<NavMeshAgent>().destination = Player.transform.position;
	}
}
