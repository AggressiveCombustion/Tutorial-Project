using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float amount = 3.0f;

    // Start is called before the first frame update
    void Start()
    {

        GameManager.instance.AddTimer(gameObject, amount, DestroySelf);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
