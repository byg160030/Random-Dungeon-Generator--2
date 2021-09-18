using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour {

	DungeonManager dungMan;

	void Start () {
		dungMan = FindObjectOfType<DungeonManager>();
		GameObject goFloor = Instantiate(dungMan.floorPrefab, transform.position, Quaternion.identity) as GameObject;
		goFloor.name = dungMan.floorPrefab.name;
		goFloor.transform.SetParent(dungMan.transform);
	}
	
	void OnDrawGizmos()
    {
		Gizmos.color = Color.white;
		Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
