using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public string moving = "";

    private Vector3 savedPosition;

    private Vector3 defaultTransform;
    public Camera cam;

    private bool canMove;
    public float movespeed = 5.4f;
    public float jumpheight = 17;
    
    private char mode;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;
    private bool onGround;

    public LayerMask whatIsWall;
    public Transform wallCheckL;
    public Transform wallCheckR;
    private bool onWall_l;
    private bool onWall_r;

    public Transform ceilCheck;
    private bool onCeil;
    private bool doubleJumpRight;
    private bool doubleJumpLeft;

    private bool facingRight = true;

    private Color zC;
    private Color xC;
    private Color cC;
    private Color vC;
    private Color invisC;

    public SpriteRenderer zInd;
    public SpriteRenderer xInd;
    public SpriteRenderer cInd;
    public SpriteRenderer vInd;

    public float boostPower = 41;
    public float leapPower = 34;


    public void switchMove(bool v)
    {
        canMove = v;
    }

    public void savePosition(Vector3 newSavedPosition)
    {
        savedPosition = newSavedPosition;
    }


    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        defaultTransform = transform.localScale;
        savedPosition = transform.position;

        canMove = true;

        zC = new Color(0.5f, 1f, 0.5f);
        xC = new Color(1f, 1f, 0.2f);
        cC = new Color(0.5f, 0.5f, 1f);
        vC = new Color(1f, 0.4f, 0.5f);
        invisC = new Color(1, 1, 1, 0);

        resetMode();
        /*
        AudioSource ac = GetComponent<AudioSource>();
        ac.PlayOneShot(ac.clip);*/
    }

    public void moveTo(string dir)
    {
        if (canMove)
        {
            rb.velocity = new Vector2(movespeed * (dir == "right" ? 1 : -1), rb.velocity.y);
            if ((dir == "right") && (!facingRight) || (dir == "left") && (facingRight))
            {
                Flip();
            }
        }
    }

    public bool makeJump(float height)
    {
        if (canMove)
        {
            float v = (float)Math.Sqrt(height * (-Physics2D.gravity.y * rb.gravityScale * 2));
            if (onGround)
            {
                rb.AddForce(new Vector2(0, v), ForceMode2D.Impulse);
                //rb.velocity = new Vector2(rb.velocity.x, power);
                return true;
            }
            else if(!onGround) //(rb.velocity.y < jumpheight)
            {
                // даблджамп
                if (doubleJumpRight && onWall_r && !onWall_l)
                {
                    doubleJumpRight = false;
                    doubleJumpLeft = true;
                    Flip();
                    rb.velocity = new Vector2(-movespeed, v);
                    return true;
                }
                else if (doubleJumpLeft && onWall_l && !onWall_r)
                {
                    doubleJumpRight = true;
                    doubleJumpLeft = false;
                    Flip();
                    rb.velocity = new Vector2(movespeed, v);
                    return true;
                    //rb.velocity = new Vector2(rb.velocity.x, power);
                }
            }
        }
        return false;
    }







    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!(!onGround && onWall_l))
            {
                moveTo("left");
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!(!onGround && onWall_r))
            {
                moveTo("right");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            makeJump(jumpheight);
        }

        //abilities
        if (Input.GetKeyDown(KeyCode.Z)) switchMode('z'); // z - squeeze
        else if (Input.GetKeyDown(KeyCode.X)) switchMode('x'); // x - speed
        else if (Input.GetKeyDown(KeyCode.C)) switchMode('c'); // c - jump
        else if (Input.GetKeyDown(KeyCode.V)) switchMode('v'); // v - power

        // reset
        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(Application.loadedLevel, LoadSceneMode.Single);
            transform.position = savedPosition;
            Debug.Log("moved");
            cam.transform.position = savedPosition;
        }
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
        onCeil = Physics2D.OverlapCircle(ceilCheck.position, groundCheckRadius-0.05f, whatIsGround);
    }

    void Flip() // flipping player's view
    {
        //sr.flipX = facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
        // Меняем местами левый и правый чек
        Vector3 tmp = wallCheckL.position;
        wallCheckL.position = wallCheckR.position;
        wallCheckR.position = tmp;
        // Вращаем свет
        /*
        tmp = lightTransform.localScale;
        lightTransform.localScale = new Vector3(tmp.x * -1, tmp.y, tmp.z);*/
    }

    void particleBurst(string mode) // boost, leap
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var vot = ps.velocityOverLifetime;
        var noise = ps.noise;
        if (mode == "size")
        {
            ps.startColor = new Color(0.20f, 0.95f, 0.23f);
            noise.strengthXMultiplier = 0;
            noise.strengthYMultiplier = 1;

            vot.enabled = true;
            vot.x = 0;
            vot.y = 0.5f;
        }
        else if (mode == "boost")
        {
            ps.startColor = new Color(0.85f, 0.85f, 0.35f);
            
            noise.strengthXMultiplier = 0;
            noise.strengthYMultiplier = 3;
            
            vot.enabled = false;
            vot.x = 0;
            vot.y = 0;
        }
        else if(mode == "leap")
        {
            ps.startColor = new Color(0.25f, 0.27f, 0.95f);

            noise.strengthXMultiplier = 3;
            noise.strengthYMultiplier = 0;

            vot.enabled = false;
            vot.x = 0;
            vot.y = 0;
        }
        else if (mode == "power")
        {
            ps.startColor = new Color(0.95f, 0.3f, 0.2f);

            noise.strengthXMultiplier = 1;
            noise.strengthYMultiplier = 3;

            vot.enabled = true;
            vot.x = facingRight ? 10 : -10;
            vot.y = 0;
        }

        ps.Stop();
        ps.Play();
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
                    transform.localScale /= 2;
                    cam.orthographicSize /= 2;
                    mode = 'z';
                    particleBurst("size");
                    switchInd('z');
                }
                else resetMode();
            }
        }

        else if (c == 'x') // --------------- speed
        {  /*
            if (mode == 'z' && !onCeil || mode != 'z')
            {
                if (mode != 'x') {
                    resetMode();
                    speedMultiplier = 1.85f;
                    mode = 'x';
                    switchInd('x');
                }
                else resetMode();
            }*/
            if (onGround)
            {
                particleBurst("boost");
                rb.velocity = new Vector2(boostPower * (facingRight ? 1 : -1), rb.velocity.y);
            }

        }

        else if (c == 'c') // --------------- jump
        {/*
            if (mode == 'z' && !onCeil || mode != 'z')
            {
                if (mode != 'c')
                {
                    resetMode();
                    mode = 'c';
                    switchInd('c');
                }
                else resetMode();
            }*/
            if (makeJump(leapPower)) {
                particleBurst("leap");
            }
        }

        else if (c == 'v') // --------------- power
        {/*            
            if (mode == 'z' && !onCeil || mode != 'z')
            {
                if (mode != 'v')
                {
                    resetMode();
                    mode = 'v';
                    switchInd('v');
                }
                else resetMode();
            }*/
            particleBurst("power");
            Vector3 pos = transform.position;
            pos.x += facingRight ? 0.5f : -0.5f;
            Collider2D[] collisions = Physics2D.OverlapCircleAll(pos, 1f, LayerMask.GetMask("PhysGround"));
            if (collisions.Length > 0)
            {
                this.rb.AddForce(new Vector2(facingRight ? -35 : 35, 3), ForceMode2D.Impulse);
                foreach (var col in collisions)
                {
                    var rb = col.GetComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    Vector3 dir = (rb.transform.position - transform.position).normalized;
                    dir.y += 0.5f;
                    dir.x += UnityEngine.Random.Range(-0.2f, 0.3f);
                    rb.AddForce(dir * 19, ForceMode2D.Impulse);
                    try
                    {
                        WeakBlock wb = col.GetComponent<WeakBlock>();
                        wb.startDestroying();
                    }
                    catch(Exception e)
                    {

                    }
                }
            }
            else
            {
                rb.AddForce(new Vector2(facingRight ? -3 : 3, 3), ForceMode2D.Impulse);
            }
        };
    }

    void resetMode()
    {
        cam.orthographicSize = 5;
        transform.localScale = new Vector3(defaultTransform.x * (facingRight ? 1 : -1), defaultTransform.y, defaultTransform.z);
        mode = 'f';
        switchInd('f');
    }
}
