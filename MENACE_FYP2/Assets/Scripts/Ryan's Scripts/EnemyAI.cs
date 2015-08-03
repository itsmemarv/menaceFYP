using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	GameObject Player;
	public GameObject gameCheckF;

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Cube(Clone)");
		//gameCheck = gameObject.GetComponent<GameCheck>();
		gameCheckF = GameObject.Find("GameCheck");
	}
	
	// Update is called once per frame
	void Update () {
		if (Player)
			GetComponent<NavMeshAgent>().destination = Player.transform.position;
		//else if (gameCheckF.GetComponent<GameCheck>().friendlyCount == 0)
		//else
			//GetComponent<NavMeshAgent>().Stop();
	}
}
