using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float timeOfArrival;
    float distance;


    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 dir = player.transform.position - transform.position;
        dir.Normalize();
        GetComponentInChildren<Transform>().rotation = Quaternion.LookRotation(dir);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.transform.position - transform.position;
        dir.Normalize();
        transform.position += (dir * distance * (1 / timeOfArrival) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            print(Vector3.SignedAngle(transform.position, player.transform.position, Vector3.up));
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "CannonBall")
        {
            Destroy(col.transform.gameObject);
            Destroy(this.gameObject);
        }
    }
}
