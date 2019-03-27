using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float gravityMult = 1f;
    public float minGroundNormalY = 0.65f;

    protected Vector2 targetVelocity;

    protected bool grounded = false;
    Vector2 groundNormal;

    protected Vector2 velocity;
    Rigidbody2D rb;

    float minMoveDist = 0.001f;

    ContactFilter2D filter;
    RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>();
    float shellRadius = 0.01f;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        filter.useTriggers = false;
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        filter.useLayerMask = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        velocity += gravityMult * Physics2D.gravity * Time.deltaTime * GameManager.instance.speed;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime * GameManager.instance.speed;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Move(move, false);

        move = Vector2.up * deltaPosition.y;

        Move(move, true);
    }

    void Move(Vector2 move, bool yMovement)
    {
        float dist = move.magnitude;
        if (dist > minMoveDist)
        {
            int count = rb.Cast(move, filter, hitBuffer, dist + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modDistance = hitBufferList[i].distance - shellRadius;
                dist = modDistance < dist ? modDistance : dist;
            }
        }

        rb.position = rb.position + move.normalized * dist;
    }
}
