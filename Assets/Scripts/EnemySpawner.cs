using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float enemyCount, overallEnemyCount, enemyKillCount;
    public Animator anim, anim2;
    public Text text;
    //[Range(-1, 1)]
   // public float ang;

    // Start is called before the first frame update
    void Start()
    {
       List<Vector3> waypoints = new List<Vector3>();

        for(float i = 1; i < 4; i++)
        {
            for (float y = 0; y < 60; y++)
            {
                
                GameObject clone = Instantiate(waypoint,
                    new Vector3(Mathf.Sin((-45f + (y * (2f * 45f / 60f))) * Mathf.Deg2Rad) * 4 * i, 0, Mathf.Cos((-45f + (y * (2f * 45f / 60f))) * Mathf.Deg2Rad) * 4 * i) + player.transform.position,
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
      if (!wait && enemyCount < 3 && overallEnemyCount < 31)
        {
            wait = true;
            enemyCount++;
            overallEnemyCount++;
            float ang = Random.Range(-1f, 1f);
            GameObject clone = Instantiate(enemy, player.transform.position + (new Vector3 (Mathf.Sin(ang), 0, Mathf.Cos(ang)) * distance), Quaternion.identity);
            clone.GetComponent<Enemy>().player = player;
            StartCoroutine(Cooldown());
        }   

      if (enemyKillCount >= 30)
        {
            anim.SetBool("Fade", true);
            text.text = "Aaaaar, that was a fine battle, matey";
            anim2.SetBool("Fade", true);
        }
   }
    
    IEnumerator Cooldown ()
    {
        yield return new WaitForSeconds(enemyRate);
        wait = false;
    }
}
