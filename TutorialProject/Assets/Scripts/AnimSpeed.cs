using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSpeed : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("GameSpeed", GameManager.instance.speed);
    }
}
