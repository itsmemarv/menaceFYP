using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileType {

	public string name;
	public GameObject goTile;

	public bool isWalkable = true;
	public float movementCost = 1;
	public int tileowner;

}
