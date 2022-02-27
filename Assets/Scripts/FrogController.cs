using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : Enemy
{
    private Rigidbody2D rb;
    private Collider2D coll;

    public Transform leftPoint, rightPoint;
    public LayerMask ground;

    private bool faceleft = true;
    public float runSpeed, jumpSpeed;
    private float leftx, rightx;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        leftx = leftPoint.position.x;
        rightx = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    void Run()
    {
        //面向左侧
        if (faceleft)
        {
            if (transform.position.x < leftx)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceleft = false;
            }
            if (coll.IsTouchingLayers(ground))
            {
                rb.velocity = new Vector2(-transform.localScale.x * runSpeed, jumpSpeed);
                anim.SetBool("jumping", true);
            }
        }
        //面向右侧
        else
        {
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceleft = true;
            }
            if (coll.IsTouchingLayers(ground))
            {
                rb.velocity = new Vector2(-transform.localScale.x * runSpeed, jumpSpeed);
                anim.SetBool("jumping", true);
            }
        }
    }

    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1f)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }
}
