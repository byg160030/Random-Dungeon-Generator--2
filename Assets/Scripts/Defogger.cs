using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defogger : MonoBehaviour
{
    public float maxLightRadius = 3f;
    public float maxVisibility = 8f;

    LayerMask wallMask, fogMask;

    void Start()
    {
        wallMask = LayerMask.GetMask("Wall");
        fogMask = LayerMask.GetMask("Fog");
    }
    public void Defog()
    {
        // send raycasts out from the player in a circular shape divided by x directions
        Vector2 rayDirection = Vector2.zero;
        int rayCount = 24;
        float angleIncrement = 360 / rayCount;
        for(int i = 0; i < rayCount; i++)
        {
            float wallDist = maxVisibility;
            float rotAngle = angleIncrement * i;
            Vector3 radialPos = transform.position + Quaternion.AngleAxis(rotAngle, Vector3.forward) * transform.up;
            rayDirection = (transform.position - radialPos).normalized;
            // find the distance of the first wall from the line of sight
            RaycastHit2D hitWall = Physics2D.Raycast(transform.position, rayDirection, Mathf.Infinity, wallMask);
            wallDist = Mathf.Min(hitWall.distance, maxVisibility);
            // detect all fog tiles with another ray of equal distance
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rayDirection, wallDist, fogMask);
            Debug.DrawRay(transform.position, rayDirection * wallDist, Color.yellow);
            foreach(RaycastHit2D hit in hits)
            {
                hit.transform.GetComponent<Fog>().isDefogged = true;
            }
        }
    }

}
