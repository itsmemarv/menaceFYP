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
	SceneController _returnNum;
	// Use this for initialization
	void Start () {
		returnNum = GameObject.Find("SceneController");
		_returnNum = returnNum.GetComponent<SceneController> ();
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
			Debug.Log("# of Player Units: " + friendlyCount);
			Debug.Log("# of Enemy Units: " + enemyCount);
		}
		else if (enemyCount == 0 && friendlyCount != 0 && victoryFlag == false)
		{
			Debug.Log("Victory");
			victoryFlag = true;
			//friendlyCount = 
//			_returnNum.Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = _returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
//			_returnNum.Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = _returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
//			_returnNum.Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
//			_returnNum.Region_SelectedRegion.GetComponent<RegionScript> ().region_Owner = _returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
//			_returnNum.Player1_Unit.GetComponent<theUnit>().numberOfUnits = friendlyCount;
//			Debug.Log("playerCount: "+_returnNum.Player1_Unit.GetComponent<theUnit>().numberOfUnits);
//			if(_returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 1){
//				_returnNum.GameMap.GetComponent<MapScript> ().player1counter ++;
//				_returnNum.GameMap.GetComponent<MapScript> ().player2counter --;
//			}
//			else if (_returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 2){
//				_returnNum.GameMap.GetComponent<MapScript> ().player2counter ++;
//				_returnNum.GameMap.GetComponent<MapScript> ().player1counter --;
//			}
			_returnNum.Player1_Unit.GetComponent<theUnit>().numberOfUnits = friendlyCount;
			Debug.Log("# of Player Remaining Units: " + friendlyCount);
			_returnNum.battleResults = 1;
			
			System.Threading.Thread.Sleep(1000);
			_returnNum.Player1_Unit = null;
			_returnNum.Player2_Unit = null;
			_returnNum.Region_SelectedRegion = null;
			_returnNum.Region_CurrentRegion = null;
			_returnNum.GameMap = null;
			_returnNum.inBattleScene = false;
			Application.LoadLevel("GameScene");
		}
		else if (enemyCount > 1 && friendlyCount == 0 && defeatFlag == false)
		{
			Debug.Log("Lose. All units dead");
			defeatFlag = true;

		
//			_returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag = _returnNum.Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().tag;
//			_returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID = _returnNum.Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
//			_returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().numberOfUnits = 1;
//			_returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().region_Owner = _returnNum.Region_SelectedRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID;
//			_returnNum.Player2_Unit.GetComponent<theUnit>().numberOfUnits = enemyCount;
//			Debug.Log("playerCount: "+_returnNum.Player1_Unit.GetComponent<theUnit>().numberOfUnits);
//			if(_returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 1){
//				_returnNum.GameMap.GetComponent<MapScript> ().player1counter ++;
//				_returnNum.GameMap.GetComponent<MapScript> ().player2counter --;
//			}
//			else if (_returnNum.Region_CurrentRegion.GetComponent<RegionScript> ().unitOnRegion.GetComponent<theUnit> ().ID == 2){
//				_returnNum.GameMap.GetComponent<MapScript> ().player2counter ++;
//				_returnNum.GameMap.GetComponent<MapScript> ().player1counter --;
//			}

			_returnNum.Player2_Unit.GetComponent<theUnit>().numberOfUnits = enemyCount;
			_returnNum.Player1_Unit.GetComponent<theUnit>().numberOfUnits = 1;
			Debug.Log("# of Enemy Remaining Units: " + enemyCount);
			Debug.Log("# of Player Remaining Units: " + _returnNum.Player1_Unit.GetComponent<theUnit>().numberOfUnits);
			_returnNum.battleResults = 2;

			System.Threading.Thread.Sleep(1000);
			_returnNum.Player1_Unit = null;
			_returnNum.Player2_Unit = null;
			_returnNum.Region_SelectedRegion = null;
			_returnNum.Region_CurrentRegion = null;
			_returnNum.GameMap = null;
			_returnNum.inBattleScene = false;
			Application.LoadLevel("GameScene");
		}
		else if (enemyCount == 0 && friendlyCount == 0 && washFlag == false)
		{
			Debug.Log("Draw");
			washFlag = true;
			_returnNum.Player1_Unit.GetComponent<theUnit>().numberOfUnits = friendlyCount + 1;
			_returnNum.Player2_Unit.GetComponent<theUnit>().numberOfUnits = enemyCount + 1;

			Debug.Log("# of Player Remaining Units: " + _returnNum.Player1_Unit.GetComponent<theUnit>().numberOfUnits);
			Debug.Log("# of Enemy Remaining Units: " + _returnNum.Player2_Unit.GetComponent<theUnit>().numberOfUnits);
			_returnNum.battleResults = 3;

			System.Threading.Thread.Sleep(1000);
			_returnNum.Player1_Unit = null;
			_returnNum.Player2_Unit = null;
			_returnNum.Region_SelectedRegion = null;
			_returnNum.Region_CurrentRegion = null;
			_returnNum.GameMap = null;
			_returnNum.inBattleScene = false;
			Application.LoadLevel("GameScene");
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
