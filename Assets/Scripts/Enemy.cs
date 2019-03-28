﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<GameObject> waypoints = new List<GameObject>();
    public GameObject player, cannonBall, particles;
    public Transform Firepoint;
    public float timeOfArrival, force, waitTime;
    float distance, d;
    bool startingToFire, fired = true;
    EnemySpawner ES;
    public int next = 0;
    public float closestDist = 200;

    // Start is called before the first frame update
    void Start()
    {
        ES = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 dir = player.transform.position - (transform.position + (-Vector3.forward * 3));
        dir.Normalize();
        GetComponentInChildren<Transform>().rotation = Quaternion.LookRotation(dir);
    }

    // Update is called once per frame
    void Update()
    {
        if (!startingToFire)
        {
            Vector3 dir = player.transform.position - (transform.position + (-Vector3.forward * 3));
            dir.Normalize();
            transform.position += (dir * distance * (1 / timeOfArrival) * Time.deltaTime);
        }

        if (Vector3.Distance(player.transform.position, transform.position) < (12 / ES.enemyCount) && !startingToFire)
        {
            startingToFire = true;
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (ES.enemyCount == 1)
                waypoints = ES.waypoints3;
            else if (ES.enemyCount == 2)
                waypoints = ES.waypoints2;
            else if (ES.enemyCount == 3)
                waypoints = ES.waypoints1;

            foreach (GameObject wp in waypoints)
            {
                if (Vector3.Distance(wp.transform.position, transform.position) < closestDist)
                {
                    closestDist = Vector3.Distance(wp.transform.position, transform.position);
                    //print(closestDist);
                    int currentIndex = waypoints.IndexOf(wp);
                    next = currentIndex;
                }
            }

            StartCoroutine(CannonF());
        }

        if (startingToFire)
        {
            Circling();
        }
    }

    void Circling()
    {
        transform.position = Vector3.MoveTowards(transform.position, NextWaypoint(), 1 * Time.deltaTime);
        Vector3 dir = NextWaypoint() - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 1 * Time.deltaTime);
        if (Vector3.Distance(NextWaypoint(), transform.position) < .2f)
        {
            AdvanceToNext();
        }

        if (!fired)
        Fire();
    }

    void Fire() {
        Vector3 dir = player.transform.position - transform.position;
        particles.SetActive(true);
        GameObject CB = Instantiate(cannonBall, Firepoint.position, Quaternion.identity);
        CB.GetComponent<Rigidbody>().AddForce(dir * force);
        CB.GetComponent<Rigidbody>().useGravity = false;
        CB.transform.tag = "EnemyCannonBall";
        CB.transform.parent = null;
        Destroy(CB, 5);
        fired = true;
        StartCoroutine(CannonF());
    }

    public Vector3 NextWaypoint()
    {
        return waypoints[next].transform.position;
    }

    public void AdvanceToNext()
    {  
            next = (next + 1) % waypoints.Count;     
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "CannonBall")
        {
            ES.enemyCount--;
            Destroy(col.transform.gameObject);
            Destroy(this.gameObject);
        }
    }

    IEnumerator CannonF ()
    {
        yield return new WaitForSeconds(waitTime);
        particles.SetActive(false);
        fired = false;
    }
}
