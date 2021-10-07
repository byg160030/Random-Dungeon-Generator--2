using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{

    public bool isDefogged;

    SpriteRenderer sr;
    Transform player;
    float maxLightRadius;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;
        maxLightRadius = player.GetComponent<Defogger>().maxLightRadius;
    }

    void Update()
    {
        FogByDistance();
    }

    void FogByDistance()
    {
        if (isDefogged)
        {
            // check distance from the player
            float dist = Vector2.Distance(transform.position, player.position);
            // if far enough away from the player... then set transparency by distance (up to 50%)
            float lightRange = Mathf.Clamp(dist, 0, maxLightRadius);
            Color tmp = sr.color;
            tmp.a = Mathf.Round(lightRange) * (0.5f / maxLightRadius);
            sr.color = tmp;
        }
    }
}
