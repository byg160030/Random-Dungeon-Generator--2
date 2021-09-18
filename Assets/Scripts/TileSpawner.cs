using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour {

	DungeonManager dungMan;

	void Start () {
		dungMan = FindObjectOfType<DungeonManager>();
	}
	
	void OnDrawGizmos()
    {
		Gizmos.color = Color.white;
		Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
