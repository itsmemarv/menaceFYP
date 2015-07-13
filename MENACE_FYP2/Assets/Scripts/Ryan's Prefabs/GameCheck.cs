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

	//Texture2D tex = new ;
	public GameObject returnNum;

	// Use this for initialization
	void Start () {
		returnNum = GameObject.Find("SceneController");
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
			//friendlyCount = 
			returnNum.GetComponent<SceneController>().Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
			returnNum.GetComponent<SceneController>().Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
			returnNum.GetComponent<SceneController>().Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
			returnNum.GetComponent<SceneController>().Region_SelectedRegion.GetComponent<RegionScript> ().region_Owner = returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
			returnNum.GetComponent<SceneController>().Player1_Unit.GetComponent<theUnit>().numberOfUnits = friendlyCount;
			Debug.Log("playerCount: "+returnNum.GetComponent<SceneController>().Player1_Unit.GetComponent<theUnit>().numberOfUnits);
			if(returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 1){
				returnNum.GetComponent<SceneController>().GameMap.GetComponent<MapScript> ().player1counter ++;
				returnNum.GetComponent<SceneController>().GameMap.GetComponent<MapScript> ().player2counter --;
			}
			else if (returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 2){
				returnNum.GetComponent<SceneController>().GameMap.GetComponent<MapScript> ().player2counter ++;
				returnNum.GetComponent<SceneController>().GameMap.GetComponent<MapScript> ().player1counter --;
			}
			
			System.Threading.Thread.Sleep(1000);
			returnNum.GetComponent<SceneController>().Player1_Unit = null;
			returnNum.GetComponent<SceneController>().Player2_Unit = null;
			returnNum.GetComponent<SceneController>().Region_SelectedRegion = null;
			returnNum.GetComponent<SceneController>().Region_CurrentRegion = null;
			returnNum.GetComponent<SceneController>().GameMap = null;
			returnNum.GetComponent<SceneController>().inBattleScene = false;
		}
		else if (enemyCount > 1 && friendlyCount == 0 && defeatFlag == false)
		{
			Debug.Log("Lose. All units dead");
			defeatFlag = true;

		
			returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = returnNum.GetComponent<SceneController>().Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
			returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = returnNum.GetComponent<SceneController>().Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
			returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
			returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().region_Owner = returnNum.GetComponent<SceneController>().Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
			returnNum.GetComponent<SceneController>().Player2_Unit.GetComponent<theUnit>().numberOfUnits = enemyCount;
			Debug.Log("playerCount: "+returnNum.GetComponent<SceneController>().Player1_Unit.GetComponent<theUnit>().numberOfUnits);
			if(returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 1){
				returnNum.GetComponent<SceneController>().GameMap.GetComponent<MapScript> ().player1counter ++;
				returnNum.GetComponent<SceneController>().GameMap.GetComponent<MapScript> ().player2counter --;
			}
			else if (returnNum.GetComponent<SceneController>().Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 2){
				returnNum.GetComponent<SceneController>().GameMap.GetComponent<MapScript> ().player2counter ++;
				returnNum.GetComponent<SceneController>().GameMap.GetComponent<MapScript> ().player1counter --;
			}

			System.Threading.Thread.Sleep(1000);
			returnNum.GetComponent<SceneController>().Player1_Unit = null;
			returnNum.GetComponent<SceneController>().Player2_Unit = null;
			returnNum.GetComponent<SceneController>().Region_SelectedRegion = null;
			returnNum.GetComponent<SceneController>().Region_CurrentRegion = null;
			returnNum.GetComponent<SceneController>().GameMap = null;
			returnNum.GetComponent<SceneController>().inBattleScene = false;
			Application.LoadLevel(0);
		}
		else if (enemyCount == 0 && friendlyCount == 0 && washFlag == false)
		{
			Debug.Log("Draw");
			washFlag = true;
			returnNum.GetComponent<SceneController>().Player1_Unit.GetComponent<theUnit>().numberOfUnits = friendlyCount + 1;
			Debug.Log("playerCount: "+returnNum.GetComponent<SceneController>().Player1_Unit.GetComponent<theUnit>().numberOfUnits);
			returnNum.GetComponent<SceneController>().Player2_Unit.GetComponent<theUnit>().numberOfUnits = enemyCount + 1;
			Debug.Log("enemyCount: "+returnNum.GetComponent<SceneController>().Player2_Unit.GetComponent<theUnit>().numberOfUnits);
			System.Threading.Thread.Sleep(1000);
			returnNum.GetComponent<SceneController>().Player1_Unit = null;
			returnNum.GetComponent<SceneController>().Player2_Unit = null;
			returnNum.GetComponent<SceneController>().Region_SelectedRegion = null;
			returnNum.GetComponent<SceneController>().Region_CurrentRegion = null;
			returnNum.GetComponent<SceneController>().GameMap = null;
			returnNum.GetComponent<SceneController>().inBattleScene = false;
			Application.LoadLevel(0);
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
