using UnityEngine;
using System.Collections;

public class GameCheck : MonoBehaviour {

	public int enemyCount = 0;
	public int friendlyCount = 0;
	private int previousCount;
	private int epreviousCount;
	private bool defeatFlag = false;
	private bool victoryFlag = false;
	private bool washFlag = false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Check();
	}

	void Check ()
	{
		previousCount = friendlyCount;
		epreviousCount = enemyCount;
		friendlyCount = GameObject.FindGameObjectsWithTag("SelectableUnits").Length;
		enemyCount = GameObject.FindGameObjectsWithTag("Eney").Length;
		if (friendlyCount != previousCount && enemyCount != epreviousCount)
		{
			Debug.Log("Player" + friendlyCount);
			Debug.Log ("Enemy" + enemyCount);
		}
		else if (enemyCount == 0 && friendlyCount != 0 && victoryFlag == false)
		{
			Debug.Log("Victory");
			victoryFlag = true;
		}
		else if (enemyCount > 1 && friendlyCount == 0 && defeatFlag == false)
		{
			Debug.Log("Game over. All units dead");
			defeatFlag = true;
		}
		else if (enemyCount == 0 && friendlyCount == 0 && washFlag == false)
		{
			Debug.Log("Draw");
			washFlag = true;
		}
	}

	/*void ChangeState ()
	{
		if (defeatFlag == true)
		{

		}
	}*/

	void OnGUI ()
	{
		//switch()
		
	}
}
