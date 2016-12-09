using UnityEngine;
using System.Collections;
using System;

public class Controls : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    private Transform transform;
    private Vector2 defaultTransform;
    private SpriteRenderer sr;
    private Camera cam;

    public float movespeed;
    public float jumpheight;
    private float jumpMultiplier = 1;
    private float speedMultiplier = 1;
    private bool jump;
    private char mode; // abilities

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool onGround;

    private bool facingRight = true;
    [HideInInspector] public string moving = "";


    private float maxt = 500;
    private float zt;
    private float xt;
    private float ct;
    private float vt;

    private Color zC;
    private Color xC;
    private Color cC;
    private Color vC;


    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        defaultTransform = transform.localScale;

        zt = maxt;
        xt = maxt;
        ct = maxt;
        vt = maxt;

        zC = new Color(0.5f, 0.5f, 0.5f);
        xC = new Color(0.7f, 0.7f, 0.2f);
        cC = new Color(0.5f, 1f, 0.5f);
        vC = new Color(1f, 0.4f, 0.5f);


    }


    public void moveRight()
    {
        
        rb.velocity = new Vector2(movespeed * speedMultiplier, rb.velocity.y);
        if (!facingRight) Flip();
    }
    public void moveLeft()
    {
        rb.velocity = new Vector2(-movespeed * speedMultiplier, rb.velocity.y);
        if (facingRight) Flip();
    }
    public void makeJump()
    {
        if(onGround) jump = true;
    }







    void Update()
    {

        switch (mode)
        {
            case 'z':
                {
                    sr.color = new Color(zC.r * (zt / (maxt + 1)), zC.g * (zt / (maxt + 1)), zC.b * (zt / (maxt + 1)));
                    if (zt-- < 0) resetMode();
                    if (xt < maxt) xt++;
                    if (ct < maxt) ct++;
                    if (vt < maxt) vt++;
                    break;
                }
            case 'x':
                {
                    sr.color = new Color(xC.r * (xt / (maxt + 1)), xC.g * (xt / (maxt + 1)), xC.b * (xt / (maxt + 1)));
                    if (xt-- < 0) resetMode();
                    if (zt < maxt) zt++;
                    if (ct < maxt) ct++;
                    if (vt < maxt) vt++;
                    break;
                }
            case 'c':
                {
                    sr.color = new Color(cC.r * (ct / (maxt + 1)), cC.g * (ct / (maxt + 1)), cC.b * (ct / (maxt + 1)));
                    if (ct-- < 0) resetMode();
                    if (xt < maxt) xt++;
                    if (zt < maxt) zt++;
                    if (vt < maxt) vt++;
                    break;
                }
            case 'v':
                {
                    sr.color = new Color(vC.r * (vt / (maxt + 1)), vC.g * (vt / (maxt + 1)), vC.b * (vt / (maxt + 1)));
                    if (vt-- < 0) resetMode();
                    if (xt < maxt) xt++;
                    if (ct < maxt) ct++;
                    if (zt < maxt) zt++;
                    break;
                }
            default:
                {
                    if (zt < maxt) zt++;
                    if (xt < maxt) xt++;
                    if (ct < maxt) ct++;
                    if (zt < maxt) vt++;
                    break;
                }
        }


        if(moving == "right")
        {
            moveRight();
        }
        if(moving == "left")
        {
            moveLeft();
        }



        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveLeft();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveRight();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            makeJump();
        }

        if (jump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpheight*jumpMultiplier);
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
    }

    void Flip() // flipping player's view
    {
        sr.flipX = facingRight;
        facingRight = !facingRight;
    }

    public void switchMode(char c)
    {
        if (c == 'z') // --------------- squeeze
        {
            if (mode != 'z')
            {
                resetMode();
                speedMultiplier = 0.8f;
                jumpMultiplier = 0.8f;
                transform.localScale = defaultTransform/2;
                mode = c;
            }
            else
            {
                resetMode();
            }
        }

        else if (c == 'x') // --------------- speed
        {
            if (mode != 'x')
            {
                resetMode();
                speedMultiplier = 1.85f;
                mode = c;
            }
            else
            {
                resetMode();
            }
        }

        else if (c == 'c') // --------------- jump
        {
            if(mode != 'c')
            {
                resetMode();
                jumpMultiplier = 1.3f;
                mode = c;
            }
            else
            {
                resetMode();
            }
        }

        else if (c == 'v') // --------------- power
        {
            if (mode != 'v')
            {
                resetMode();
                mode = c;
            }
            else
            {
                resetMode();
            }
        };
    }

    void resetMode()
    {
        sr.color = new Color(1f, 1f, 1f);
        jumpMultiplier = 1;
        speedMultiplier = 1;
        transform.localScale = defaultTransform;
        mode = 'f';
    }
}
