using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	public GameObject bulletObject;

	public float Speed = 40.0f;
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime * Speed, Space.World);
	}

	void OnTriggerStay(Collider cd)
	{
		if (cd.gameObject.tag == "SelectableUnits")//(cd.tag.Equals("Eney"))
		{
			GameObject g = (GameObject)Instantiate(bulletObject, transform.position, Quaternion.identity);
			g.GetComponent<Bullet>().target = cd.transform;
		}
	}
}
