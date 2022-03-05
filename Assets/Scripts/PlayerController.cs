using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    public AudioSource jumpAudio;
    public AudioSource hurtAudio;
    public AudioSource cherryAudio;
    public Transform groundCheck;

    public float runSpeed;
    public float jumpSpeed;
    public float climbSpeed;
    public int jumpNumber;
    public LayerMask ground;

    public int cherry = 0;
    public Text cherryNumber;

    private bool isHurt;//默认是false
    private bool isLadder;
    private bool isClimbing;
    private float playerGravity;
    private bool isGround, isJump;
    private bool jumpPressed;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        playerGravity = rb.gravityScale;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpNumber > 0)
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        if (!isHurt)
            Run();
        Jump();
        SwitchAnim();
        CheckLadder();
        Climb();
    }

    //角色移动
    void Run()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * runSpeed, rb.velocity.y);
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
            //anim.SetFloat("running", Mathf.Abs(horizontalMove));
        }
    }

    //跳跃
    void Jump()
    {
        if (isGround)
        {
            jumpNumber = 2;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            jumpAudio.Play();
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            jumpNumber--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpNumber > 0 && isJump)
        {
            jumpAudio.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            jumpNumber--;
            jumpPressed = false;
        }
    }

    //爬梯子
    void Climb()
    {
        if (isLadder)
        {
            float moveY = Input.GetAxis("Vertical");
            if (moveY > 0.5f || moveY < -0.5f)
            {
                anim.SetBool("climbing", true);
                rb.gravityScale = 0.0f;
                rb.velocity = new Vector2(rb.velocity.x, moveY * climbSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            }
        }
        else
        {
            anim.SetBool("climbing", false);
            rb.gravityScale = playerGravity;
        }
    }
    void CheckLadder()
    {
        isLadder = coll.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }

    //动画切换
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));
        if (isGround)
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
        else if(isJump && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        if (isHurt)
        {
            anim.SetBool("hurting", true);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurting", false);
                anim.SetFloat("running", 0);
                isHurt = false;
            }
        }
    }

    //碰撞触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集物品
        if (collision.tag == "Collection")
        {
            cherryAudio.Play();
            Destroy(collision.gameObject);
            cherry += 1;
            cherryNumber.text = cherry.ToString();
        }
        //死亡
        if (collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 1f);
        }
    }

    //触碰敌人（消灭+受伤）
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
            //受伤
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10f, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10f, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }
        }

    }

    //重新加载场景
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
