using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Units : MonoBehaviour {

	public List<GameObject> _list;

	public GameObject prefab;
	public GameObject sceneController;
	theUnit unit;
	public int unitNumbers;
	private int widthToPass;
	private int heightToPass;
	public int columnLimit = 1;
	public float spacing = 1.5f;
	public float xpos;
	public float ypos;
	public float zpos;
	public Vector3 EnteredCoord;

	public float spawnSpeed = 0.0f;

	//private int addMod;

	// Use this for initialization
	void Start () {
		sceneController = GameObject.Find("SceneController");

		if (prefab.gameObject.tag == "SelectableUnits")
		{
			widthToPass = sceneController.GetComponent<SceneController>().Player1_Unit.GetComponent<theUnit>().numberOfUnits;
			//Mathf.RoundToInt(widthToPass);
			unitNumbers = widthToPass;
		}
		else if (prefab.gameObject.tag == "Eney")
		{
			widthToPass = sceneController.GetComponent<SceneController>().Player2_Unit.GetComponent<theUnit>().numberOfUnits;
			//Mathf.RoundToInt(widthToPass);
			unitNumbers = widthToPass;
		}

		//heightToPass = sceneController.GetComponent<SceneController>().Player1_Unit.GetComponent<theUnit>().numberOfUnits / 2;
		//Mathf.RoundToInt(heightToPass);
		//columnLimit = heightToPass;
		//Debug.Log(unitWidth);

		_list = new List<GameObject>();

		EnteredCoord = new Vector3(xpos, ypos, zpos);
		//for(int i = 0; i < (unitWidth * unitHeight); ++i)
		//{
		for(int x = 0; x < unitNumbers; x++){
			for(int z = 0; z < columnLimit; z++){
				Vector3 pos = new Vector3(x, 0.35f, z) * spacing;
				_list.Add ((GameObject)Instantiate(prefab, pos + EnteredCoord, Quaternion.identity));
			}
		}
		//}
		//StartCoroutine(CreateSquad());
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position.Set(xpos, ypos, zpos);
	}

	/*IEnumerator CreateSquad() {

		for(int x = 0; x < unitNumbers; x++) {
			yield return new WaitForSeconds(spawnSpeed);
			
			for(int z = 0; z < columnLimit; z++) {                
				yield return new WaitForSeconds(spawnSpeed);

				Vector3 pos = new Vector3(x, 0, z) * spacing;
				Instantiate(prefab, pos, Quaternion.identity);
				//GameObject block = Instantiate(unit, Vector3.zero, unit.transform.rotation) as GameObject;
				//block.transform.parent = transform;
				//block.transform.localPosition = new Vector3(x, 0, z) * spacing;
			}
			yield return null;
		}
	}*/
}
