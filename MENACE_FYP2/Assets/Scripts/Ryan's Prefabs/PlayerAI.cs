using UnityEngine;
using System.Collections;

public class PlayerAI : MonoBehaviour {

	GameObject Enemy;
	public GameObject gameCheck;

	// Use this for initialization
	void Start () {
		Enemy = GameObject.Find("Enemy(Clone)");
		//gameCheck = gameObject.GetComponent<GameCheck>();
		gameCheck = GameObject.Find("GameCheck");
	}
	
	// Update is called once per frame
	void Update () {
		if (Enemy)
			GetComponent<NavMeshAgent>().destination = Enemy.transform.position;
		//else if (gameCheck.GetComponent<GameCheck>().enemyCount == 0)
		//else
			//GetComponent<NavMeshAgent>().Stop();
	}
}
