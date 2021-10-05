using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Vector2 patrolInterval;

    LayerMask obstacleMask;
    Vector2 curPos;
    List<Vector2> availableMovementList = new List<Vector2>();
    bool isMoving;

    void Start()
    {
        obstacleMask = LayerMask.GetMask("Wall", "Enemy", "Player");
        curPos = transform.position;
    }

    void Update()
    {
        if (!isMoving)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        availableMovementList.Clear();
        Vector2 size = Vector2.one * 0.8f;
        Collider2D hitUp = Physics2D.OverlapBox(curPos + Vector2.up, size, 0, obstacleMask);
        if (!hitUp) {availableMovementList.Add(Vector2.up);}
        Collider2D hitRight = Physics2D.OverlapBox(curPos + Vector2.right, size, 0, obstacleMask);
        if (!hitRight) { availableMovementList.Add(Vector2.right); }
        Collider2D hitDown = Physics2D.OverlapBox(curPos + Vector2.down, size, 0, obstacleMask);
        if (!hitDown) { availableMovementList.Add(Vector2.down); }
        Collider2D hitLeft = Physics2D.OverlapBox(curPos + Vector2.left, size, 0, obstacleMask);
        if (!hitLeft) { availableMovementList.Add(Vector2.left); }
        if(availableMovementList.Count > 0)
        {
            int randomIndex = Random.Range(0, availableMovementList.Count);
            curPos += availableMovementList[randomIndex];
        }
        StartCoroutine(SmoothMove());
    }
    IEnumerator SmoothMove()
    {
        isMoving = true;
        while (Vector2.Distance(transform.position, curPos) > 0.01f) {
            transform.position = Vector2.MoveTowards(transform.position, curPos, 5f * Time.deltaTime);
            yield return null;
        }
        transform.position = curPos;
        yield return new WaitForSeconds(Random.Range(patrolInterval.x, patrolInterval.y));
        isMoving = false;
    }


}
