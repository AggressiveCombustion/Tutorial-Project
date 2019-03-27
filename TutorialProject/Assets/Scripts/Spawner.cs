using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float rate = 1.0f;
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(gameObject, rate, Spawn);
    }

    void Spawn()
    {
        Instantiate(obj, transform.position, transform.rotation);
        GameManager.instance.AddTimer(gameObject, rate, Spawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
