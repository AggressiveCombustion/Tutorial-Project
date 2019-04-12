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
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * GameManager.instance.speed;
    }

    public void DestroySelf()
    {
        Instantiate(burst, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
