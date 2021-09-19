using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour {

	public GameObject floorPrefab, wallPrefab, tilePrefab;

	[HideInInspector] public float minX, maxX, minY, maxY;

    private List<Vector3> floorList = new List<Vector3>();
    public int totalFloorCount;

    void Start()
    {
        RandomWalker();
    }

    void Update()
    {
       if(Application.isEditor && Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void RandomWalker()
    {
        Vector3 curPos = Vector3.zero;
        //set floor tile at curPos
        floorList.Add(curPos);
        while (floorList.Count < totalFloorCount)
        {
            switch(Random.Range(1, 5))
            {
                case 1: curPos += Vector3.up;   break;
                case 2: curPos += Vector3.right;break;
                case 3: curPos += Vector3.down; break;
                case 4: curPos += Vector3.left; break;
            }
            bool inFloorList = false;
            for(int i = 0; i < floorList.Count; i++)
            {
                if(Vector3.Equals(curPos, floorList[i]))
                {
                    inFloorList = true;
                    break;
                }
            }
            if (!inFloorList)
            {
                floorList.Add(curPos);
            }
        }
        for(int i = 0; i < floorList.Count; i++)
        {
            GameObject goTile = Instantiate(tilePrefab, floorList[i], Quaternion.identity) as GameObject;
            goTile.name = tilePrefab.name;
            goTile.transform.SetParent(transform);
        }
    }
}
