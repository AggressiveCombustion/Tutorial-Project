using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 3.0f;
    public float jumpForce = 5.0f;
    
    float facing = 1;

    public float killY = -10;

    public SpriteRenderer sprite;
    Animator anim;

    public GameObject dustTrail;
    public int dustEmissionRate = 5;
    
    Vector3 startPos;

    public Transform height;

    bool prev_grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = sprite.GetComponent<Animator>();

        startPos = transform.position;
    }

    public new void Update()
    {
        base.Update();
        anim.SetFloat("moveSpeed", Mathf.Abs(velocity.x));
        sprite.flipX = facing != 1;

        prev_grounded = grounded;
    }
    
    protected override void ComputeVelocity()
    {
        HandleDust();

        if(velocity.y < 0)
        //if (!grounded)
            gravityMult += 5f * Time.deltaTime;
        
        else
            gravityMult = 1;

        if (gravityMult > 2)
            gravityMult = 2;

        Vector2 move = Vector2.zero;
        float h_input = Input.GetAxis("Horizontal");

        move.x = h_input;
        if (move.x != 0)
            facing = Mathf.Sign(move.x);

        anim.SetBool("grounded", grounded);
        

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("attack");
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpForce;
            height.GetComponent<Animator>().SetTrigger("stretch");
        }

        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * .5f;
            }
        }

        targetVelocity = move * maxSpeed;

        // check if landing
        if (grounded && !prev_grounded)
            height.GetComponent<Animator>().SetTrigger("squash");

        // death by fall
        if (transform.position.y < killY)
        {
            DeathByFall();
        }


    }

    void HandleDust()
    {
        ParticleSystem ps = dustTrail.GetComponent<ParticleSystem>();
        var emission = ps.emission;
        emission.rateOverDistance = velocity.y >= 0 ? 5 : 0;
    }

    public void TakeDamage(float amount)
    {
        GetComponent<Damageable>().TakeDamage(amount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Fireball>() != null)
        {
            TakeDamage(1);
            collision.gameObject.GetComponent<Fireball>().DestroySelf();
        }
    }

    public void DeathByFall()
    {
        GameManager.instance.AddTimer(gameObject, 0.5f, ResetPosition);
    }

    public void ResetPosition()
    {
        transform.position = startPos;
    }
}
