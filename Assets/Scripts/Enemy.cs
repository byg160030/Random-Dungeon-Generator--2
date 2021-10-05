using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2 dmgRange;
    public float chaseSpeed;
    public float alertRange;
    public Vector2 patrolInterval;

    Player player;
    LayerMask obstacleMask, walkableMask;
    Vector2 curPos;
    List<Vector2> availableMovementList = new List<Vector2>();
    List<Node> nodesList = new List<Node>();
    bool isMoving;

    void Start()
    {
        player = FindObjectOfType<Player>();
        obstacleMask = LayerMask.GetMask("Wall", "Enemy", "Player");
        walkableMask = LayerMask.GetMask("Wall", "Enemy");
        curPos = transform.position;
        StartCoroutine(Movement());
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
        StartCoroutine(SmoothMove(Random.Range(patrolInterval.x, patrolInterval.y)));
    }
    IEnumerator SmoothMove(float speed)
    {
        isMoving = true;
        while (Vector2.Distance(transform.position, curPos) > 0.01f) {
            transform.position = Vector2.MoveTowards(transform.position, curPos, 5f * Time.deltaTime);
            yield return null;
        }
        transform.position = curPos;
        yield return new WaitForSeconds(speed);
        isMoving = false;
    }

    void Attack()
    {
        int roll = Random.Range(0, 100);
        if(roll > 50)
        {
            float dmgAmount = Mathf.Ceil(Random.Range(dmgRange.x, dmgRange.y));
            Debug.Log(name + " attack and hit for " + dmgAmount + " points of damage");
        }
        else
        {
            Debug.Log(name + " attack and missed");
        }
    }

    void CheckNode(Vector2 chkPoint, Vector2 parent)
    {
        Vector2 size = Vector2.one * 0.5f;
        Collider2D hit = Physics2D.OverlapBox(chkPoint, size, 0, walkableMask);
        if (!hit)
        {
            nodesList.Add(new Node(chkPoint, parent));
        }
    }

    Vector2 FindNextStep(Vector2 startPos, Vector2 targetPos)
    {
        int listIndex = 0;
        Vector2 myPos = startPos;
        nodesList.Clear();
        nodesList.Add(new Node(startPos, startPos));
        while (myPos != targetPos && listIndex < 1000 && nodesList.Count > 0)
        {
            // check up, down, left  & right (if walkable then add to list)
            CheckNode(myPos + Vector2.up, myPos);
            CheckNode(myPos + Vector2.right, myPos);
            CheckNode(myPos + Vector2.down, myPos);
            CheckNode(myPos + Vector2.left, myPos);
            listIndex++;
            if(listIndex < nodesList.Count)
            {
                myPos = nodesList[listIndex].position;
            }
        }
        if(myPos == targetPos)
        {
            nodesList.Reverse(); // crawl backwards through nodes list
            for(int i = 0; i < nodesList.Count; i++)
            {
                if(myPos == nodesList[i].position)
                {
                    if(nodesList[i].parent == startPos)
                    {
                        return myPos;
                    }
                    myPos = nodesList[i].parent;
                }
            }
        }
        return startPos;
    }

    IEnumerator Movement()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!isMoving)
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if(dist <= alertRange)
                {
                    if(dist <= 1.1f)
                    {
                        Attack();
                        yield return new WaitForSeconds(Random.Range(0.5f, 1.15f));
                    }
                    else
                    {
                        Vector2 newPos = FindNextStep(transform.position, player.transform.position);
                        if(newPos != curPos)
                        {
                            // chase
                            curPos = newPos;
                            StartCoroutine(SmoothMove(chaseSpeed));
                        }
                        else
                        {
                            Patrol();
                        }
                    }
                }
                else
                {
                    Patrol();
                }
            }
        }
    }

}
