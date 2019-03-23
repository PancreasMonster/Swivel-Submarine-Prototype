using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{
    public GameObject NetworkUnit;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(NetworkUnit);
        Debug.Log("Unit Spawned");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
