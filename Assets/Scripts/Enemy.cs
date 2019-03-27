using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float timeOfArrival;
    float distance, d;
    bool startingToFire;
    EnemySpawner ES;

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

        if (Vector3.Distance(player.transform.position, transform.position) < (10 / ES.enemyCount) && !startingToFire)
        {
            startingToFire = true;
            distance = Vector3.Distance(transform.position, player.transform.position);
        }

        if (startingToFire)
        {
           
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            print(Vector3.SignedAngle(transform.position, player.transform.position, Vector3.up));
        }
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
}
