using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Units : MonoBehaviour {

	public List<GameObject> _list;

	public GameObject prefab;

	public int unitWidth = 5;
	public int unitHeight = 5;
	public float spacing = 1.5f;

	public float spawnSpeed = 0.0f;

	// Use this for initialization
	void Start () {
		_list = new List<GameObject>();

		//for(int i = 0; i < (unitWidth * unitHeight); ++i)
		//{
			for(int x = 0; x < unitWidth; x++){
				for(int z = 0; z < unitHeight; z++){
					Vector3 pos = new Vector3(x, 0, z) * spacing;
					_list.Add ((GameObject)Instantiate(prefab, pos, Quaternion.identity));
				}
			}
		//}
		//StartCoroutine(CreateSquad());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator CreateSquad() {

		for(int x = 0; x < unitWidth; x++) {
			yield return new WaitForSeconds(spawnSpeed);
			
			for(int z = 0; z < unitHeight; z++) {                
				yield return new WaitForSeconds(spawnSpeed);

				Vector3 pos = new Vector3(x, 0, z) * spacing;
				Instantiate(prefab, pos, Quaternion.identity);
				//GameObject block = Instantiate(unit, Vector3.zero, unit.transform.rotation) as GameObject;
				//block.transform.parent = transform;
				//block.transform.localPosition = new Vector3(x, 0, z) * spacing;
			}
			yield return null;
		}
	}
}
