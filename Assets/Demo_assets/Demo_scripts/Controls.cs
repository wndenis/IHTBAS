using UnityEngine;
using System.Collections;
using System;

public class Controls : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    //private Transform transform;
    private Vector2 defaultTransform;
    private Camera cam;

    public void switchMove(bool v)
    {
        canMove = v;
    }

    private bool canMove;
    public float movespeed;
    public float jumpheight;
    private float jumpMultiplier = 1;
    private float speedMultiplier = 1;
    [HideInInspector] public bool jump;
    private char mode;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;
    private bool onGround;

    public LayerMask whatIsWall;
    public Transform wallCheckL;
    public Transform wallCheckR;
    //public float wallCheckRadius;
    private bool onWall_l;
    private bool onWall_r;
    private bool doubleJumpRight;
    private bool doubleJumpLeft;

    public Transform ceilCheck;
    private bool onCeil;

    private bool facingRight = true;
    [HideInInspector] public string moving = "";

    private Color zC;
    private Color xC;
    private Color cC;
    private Color vC;
    private Color invisC;

    public SpriteRenderer zInd;
    public SpriteRenderer xInd;
    public SpriteRenderer cInd;
    public SpriteRenderer vInd;




    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        defaultTransform = transform.localScale;

        canMove = true;

        zC = new Color(0.5f, 1f, 0.5f);
        xC = new Color(1f, 1f, 0.2f);
        cC = new Color(0.5f, 0.5f, 1f);
        vC = new Color(1f, 0.4f, 0.5f);
        invisC = new Color(1, 1, 1, 0);

        AudioSource ac = GetComponent<AudioSource>();
        ac.PlayOneShot(ac.clip);
    }


    public void moveRight()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(movespeed * speedMultiplier, rb.velocity.y);
            if (!facingRight) Flip();
        }
    }
    public void moveLeft()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(-movespeed * speedMultiplier, rb.velocity.y);
            if (facingRight) Flip();
        }
    }

    public void makeJump()
    {
        if (canMove)
        {
            if (onGround)
            {
                jump = true;
            }
            else /*if(rb.velocity.y < jumpheight)*/
            {
                if (onWall_r) {
                    if(facingRight && doubleJumpRight)
                    {
                        Debug.Log("Fr = " + facingRight);
                        Debug.Log("DJL = " + doubleJumpLeft + ";  DJR = " + doubleJumpRight);
                        Debug.Log("\n\n\n\n");
                        doubleJumpRight = false;
                        doubleJumpLeft = true;
                        jump = true;
                    }
                    else if (!facingRight && doubleJumpLeft)
                    {
                        doubleJumpRight = true;
                        doubleJumpLeft = false;
                        jump = true;
                    }
                }
            }
        }
    }







    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!(!onGround && onWall_l))
            {
                moveLeft();
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!(!onGround && onWall_r))
            {
                moveRight();
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            makeJump();
        }



        if (moving == "right")
        {
            moveRight();
        }
        if (moving == "left")
        {
            moveLeft();
        }
        if (jump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpheight * jumpMultiplier);
            jump = false;
        }

        //abilities
        if (Input.GetKeyDown(KeyCode.Z)) switchMode('z'); // z - squeeze
        else if (Input.GetKeyDown(KeyCode.X)) switchMode('x'); // x - speed
        else if (Input.GetKeyDown(KeyCode.C)) switchMode('c'); // c - jump
        else if (Input.GetKeyDown(KeyCode.V)) switchMode('v'); // v - power
    }

    void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (onGround)
        {
            doubleJumpRight = true;
            doubleJumpLeft = true;
        }
        onWall_l = Physics2D.OverlapCircle(wallCheckL.position, groundCheckRadius, whatIsWall);
        onWall_r = Physics2D.OverlapCircle(wallCheckR.position, groundCheckRadius, whatIsWall);
        onCeil = Physics2D.OverlapCircle(ceilCheck.position, groundCheckRadius, whatIsWall);
    }

    void Flip() // flipping player's view
    {
        //sr.flipX = facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
    }

    void switchInd(char ind)
    {
        zInd.color = invisC;
        xInd.color = invisC;
        cInd.color = invisC;
        vInd.color = invisC;
        switch (ind)
        {
            case 'z':{
                    zInd.color = zC;
                    break;
                }
            case 'x': {
                    xInd.color = xC;
                    break;
                }
            case 'c': {
                    cInd.color = cC;
                    break;
                }
            case 'v': {
                    vInd.color = vC;
                    break;
                }
        }
    }

    public void switchMode(char c)
    {
        if (c == 'z') // --------------- squeeze
        {
            if (mode == 'z' && !onCeil || mode != 'z')
            {
                if (mode != 'z')
                {
                    resetMode();
                    speedMultiplier = 0.8f;
                    jumpMultiplier = 0.8f;
                    transform.localScale /= 2;
                    mode = 'z';
                    switchInd('z');
                }
                else resetMode();
            }
        }

        else if (c == 'x') // --------------- speed
        {  
            if (mode == 'z' && !onCeil || mode != 'z')
            {
                if (mode != 'x') {
                    resetMode();
                    speedMultiplier = 1.85f;
                    mode = 'x';
                    switchInd('x');
                }
                else resetMode();
            }
        }

        else if (c == 'c') // --------------- jump
        {
            if (mode == 'z' && !onCeil || mode != 'z')
            {
                if (mode != 'c')
                {
                    resetMode();
                    jumpMultiplier = 1.3f;
                    mode = 'c';
                    switchInd('c');
                }
                else resetMode();
            }
        }

        else if (c == 'v') // --------------- power
        {
            
            if (mode == 'z' && !onCeil || mode != 'z')
            {
                if (mode != 'v')
                {
                    resetMode();
                    mode = 'v';
                    switchInd('v');
                }
                else resetMode();
            }
        };
    }

    void resetMode()
    {
        jumpMultiplier = 1;
        speedMultiplier = 1;
        transform.localScale = new Vector3(defaultTransform.x * (facingRight ? 1 : -1), defaultTransform.y, transform.localScale.z);
        mode = 'f';
        switchInd('f');
    }
}
