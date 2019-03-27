using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public float distance, enemyRate;
    bool wait;
    public float enemyCount;
    //[Range(-1, 1)]
   // public float ang;

    // Start is called before the first frame update
    void Start()
    {
       
        for(int i = 1; i < 4; i++)
        {
            for (int y = 0; y < 12; y++)
            {
                GameObject clone = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere),
                    new Vector3(Mathf.Sin(-0.701f + (y * (2 * .701f / 6))) * 4 * i, 0, Mathf.Cos(-0.701f + (y * (2 * 0.701f / 6))) * 4 * i) + player.transform.position,
                    Quaternion.identity);
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
