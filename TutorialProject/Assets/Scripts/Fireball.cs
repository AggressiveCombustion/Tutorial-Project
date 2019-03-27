using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Vector3 speed = new Vector3(1, 0, 0);
    public GameObject burst;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddTimer(gameObject, 5.0f, DestroySelf);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime;
    }

    public void DestroySelf()
    {
        Instantiate(burst, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
