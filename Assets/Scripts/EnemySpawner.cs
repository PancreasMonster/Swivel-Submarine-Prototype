using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public float distance;
    //[Range(-1, 1)]
   // public float ang;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
        {
            float ang = Random.Range(-1f, 1f);
            GameObject clone = Instantiate(enemy, player.transform.position + (new Vector3 (Mathf.Sin(ang), 0, Mathf.Cos(ang)) * distance), Quaternion.identity);
            clone.GetComponent<Enemy>().player = player;
        }   
    }
}
