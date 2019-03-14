using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerInput : MonoBehaviour
{
    public float speed, forceNeeded, cooldownLenght;
    bool cooldown;

    void Update()
    {
        //transform.Translate(Input.acceleration.x * Time.deltaTime * speed, 0, 0);
        print(Input.acceleration.x);
        if (Input.acceleration.x > forceNeeded && !cooldown)
        {
            transform.Translate(0, 1, 0);
            cooldown = true;
            StartCoroutine(BeenSwung());
        }
    }

    IEnumerator BeenSwung ()
    {
        yield return new WaitForSeconds(cooldownLenght);
        cooldown = false;
    }
}
