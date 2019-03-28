﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> waypoints1 = new List<GameObject>();
    public List<GameObject> waypoints2 = new List<GameObject>();
    public List<GameObject> waypoints3 = new List<GameObject>();
    public GameObject enemy;
    public GameObject player;
    public float distance, enemyRate;
    bool wait;
    public GameObject waypoint;
    public float enemyCount;
    //[Range(-1, 1)]
   // public float ang;

    // Start is called before the first frame update
    void Start()
    {
       List<Vector3> waypoints = new List<Vector3>();

        for(int i = 1; i < 4; i++)
        {
            for (int y = 0; y < 60; y++)
            {
                
                GameObject clone = Instantiate(waypoint,
                    new Vector3(Mathf.Sin((-60 + (y * (2 * 60 / 60))) * Mathf.Deg2Rad) * 4 * i, 0, Mathf.Cos((-60 + (y * (2 * 60 / 60))) * Mathf.Deg2Rad) * 4 * i) + player.transform.position,
                    Quaternion.identity);

               

                clone.name = "WP " + y.ToString() + " " + i.ToString();
                clone.transform.SetParent(transform);
                if (i == 1)
                    waypoints1.Add(clone);
                else if (i == 2)
                    waypoints2.Add(clone);
                else if (i == 3)
                {
                    waypoints3.Add(clone);
                }
            

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
      if (!wait && enemyCount < 3)
        {
            wait = true;
            enemyCount++;
            float ang = Random.Range(-1f, 1f);
            GameObject clone = Instantiate(enemy, player.transform.position + (new Vector3 (Mathf.Sin(ang), 0, Mathf.Cos(ang)) * distance), Quaternion.identity);
            clone.GetComponent<Enemy>().player = player;
            StartCoroutine(Cooldown());
        }   
   }
    
    IEnumerator Cooldown ()
    {
        yield return new WaitForSeconds(enemyRate);
        wait = false;
    }
}
