using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Transform trfm;
    private TrailRenderer tr;

    private Vector3 savedPosition;

    private Vector3 defaultTransform;
    private float defaultCamTransform;

    public Camera cam;
    
    public float movespeed = 5.6f;
    public float jumpheight = 17;
    
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    public Transform groundCheck;
    public float groundCheckRadius;
    public Transform wallCheckL;
    public Transform wallCheckR;
    public Transform ceilCheck;

    public float boostPower = 41;
    public float leapPower = 34;

    private bool canMove;
    private bool canJump;
    private float trailTime;
    private float sizeK = 1;

    private bool onGround;
    private bool onWall_l;
    private bool onWall_r;
    private bool onCeil;

    private bool doubleJumpRight;
    private bool doubleJumpLeft;

    private bool facingRight = true;

    public Color zC;
    public Color xC;
    public Color cC;
    public Color vC;
    public Color invisC;

    public int sizeCharges = 0;
    public int boostCharges = 0;
    public int leapCharges = 0;
    public int powerCharges = 0;

    private bool sizeMode = false;
    private bool boostMode = false;
    private bool leapMode = false;
    private bool powerMode = false;

    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        trfm = GetComponent<Transform>();
        tr = GetComponent<TrailRenderer>();

        defaultTransform = trfm.localScale;
        defaultCamTransform = cam.orthographicSize;
        savedPosition = trfm.position;

        canMove = true;
        canJump = true;
        trailTime = 0;

        zC = new Color(0.5f, 1f, 0.5f);
        xC = new Color(1f, 1f, 0.2f);
        cC = new Color(0.5f, 0.5f, 1f);
        vC = new Color(1f, 0.4f, 0.5f);
        invisC = new Color(0.5f, 0.5f, 0.5f, 0);

        resetMode();
        /*
        AudioSource ac = GetComponent<AudioSource>();
        ac.PlayOneShot(ac.clip);*/
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
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Single);
            trfm.position = savedPosition;
            cam.transform.position = savedPosition;
        }

        CheckModes();
        CheckTrail();

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
        onCeil = Physics2D.OverlapCircle(ceilCheck.position, groundCheckRadius - 0.05f, whatIsGround);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }




    public void moveTo(string dir)
    {
        if (canMove)
        {
            rb.velocity = new Vector2(movespeed * (dir == "right" ? 1 : -1) * sizeK, rb.velocity.y);
            /*if (Math.Abs(rb.velocity.x) < maxSpeed)
            {
                rb.AddForce(new Vector2(movespeed * (dir == "right" ? 1 : -1), 0));
            }*/
            //rb.MovePosition(new Vector2(transform.position.x + movespeed * (dir == "right" ? 1 : -1) / 2, transform.position.y));
            if ((dir == "right") && (!facingRight) || (dir == "left") && (facingRight))
            {
                Flip();
            }
        }
    }

    public bool makeJump(float height)
    {
        if (canJump)
        {
            float v = (float)Math.Sqrt(height * sizeK * (-Physics2D.gravity.y * rb.gravityScale * 2.3f));
            if (onGround)
            {
                rb.AddForce(new Vector2(0, v), ForceMode2D.Impulse);
                //rb.velocity = new Vector2(rb.velocity.x, power);
                return true;
            }
            else if (!onGround) //(rb.velocity.y < jumpheight)
            {
                // даблджамп
                if (doubleJumpRight && onWall_r && !onWall_l)
                {
                    doubleJumpRight = false;
                    doubleJumpLeft = true;
 
                    rb.velocity = new Vector2(-movespeed, v) * sizeK;
                    Flip();
                    return true;
                }
                else if (doubleJumpLeft && onWall_l && !onWall_r)
                {
                    doubleJumpRight = true;
                    doubleJumpLeft = false;

                    rb.velocity = new Vector2(movespeed, v) * sizeK;
                    Flip();
                    return true;
                }
            }
        }
        return false;
    }

    void Flip() // flipping player's view
    {
        //sr.flipX = facingRight;
        trfm.localScale = new Vector3(trfm.localScale.x * -1, trfm.localScale.y, trfm.localScale.z);
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

    private void CheckTrail()
    {
        if ((trailTime -= Time.deltaTime) <= 0)
        {
            trailTime = 0;
            tr.enabled = false;
            canMove = true;
        }
    }

    private void CheckModes()
    {
        if (boostMode && Math.Abs(rb.velocity.x) < 1) // если после рывка воткнулись в стену, то рывок останавливается
        {
            boostMode = false;
            trailTime = 0;
        }
        
    }

    void particleBurst(string mode) // boost, leap
    {
        //tr.colorGradient.alphaKeys[0] = new GradientAlphaKey(1, 0);
        tr.enabled = true;
        if (mode == "size")
        {
            tr.startColor = zC;
        }
        else if (mode == "boost")
        {
            canMove = false;
            tr.startColor = xC;
            trailTime = 0.5f;
        }
        else if(mode == "leap")
        {
            tr.startColor = cC;
            trailTime = 1.1f;
        }
        else if (mode == "power")
        {
            tr.startColor = vC;
            trailTime = 1.1f;
        }
        
    }

    public void switchMode(char c)
    {
        if (c == 'z') // --------------- squeeze
        {
            if (!sizeMode && sizeCharges > 0)
            {
                resetMode();
                sizeCharges--;
                trfm.localScale /= 2;
                cam.orthographicSize /= 2;
                tr.startWidth /= 2;
                sizeK = 0.65f;
                sizeMode = true;
                particleBurst("size");
            }
            else if(sizeMode && !onCeil)
            resetMode();
        }

        else if (c == 'x') // --------------- speed
        {  
            if (onGround && boostCharges > 0)
            {
                boostMode = true;
                boostCharges--;
                particleBurst("boost");
                rb.velocity = new Vector2(boostPower * (facingRight ? 1 : -1), rb.velocity.y);
            }
        }

        else if (c == 'c') // --------------- jump
        {
            if (leapCharges > 0 && makeJump(leapPower) ) {
                leapMode = true;
                leapCharges--;
                particleBurst("leap");
            }
        }

        else if (c == 'v') // --------------- power
        {
            if (powerCharges > 0)
            {
                powerMode = true;
                powerCharges--;
                particleBurst("power");
                Vector3 pos = trfm.position;
                pos.x += facingRight ? 0.5f : -0.5f;
                Collider2D[] collisionsPhys = Physics2D.OverlapCircleAll(pos, 1.1f, LayerMask.GetMask("PhysGround"));
                Collider2D[] collisionsGround = Physics2D.OverlapCircleAll(pos, 0.15f, LayerMask.GetMask("Ground"));
                if (collisionsPhys.Length > 0)
                {
                    this.rb.AddForce(new Vector2(facingRight ? -3 : 3, 3), ForceMode2D.Impulse);
                    foreach (var col in collisionsPhys)
                    {
                        var rb = col.GetComponent<Transform>();
                        //rb.bodyType = RigidbodyType2D.Dynamic;
                        Vector3 dir = (rb.transform.position - trfm.position).normalized;
                        dir.y += 0.5f;
                        dir.x += UnityEngine.Random.Range(-0.2f, 0.3f);

                        try
                        {
                            var rbblock = col.GetComponent<Rigidbody2D>();
                            if(rbblock)
                                rbblock.AddForce(dir * 19, ForceMode2D.Impulse);
                            WeakBlock wb = col.GetComponent<WeakBlock>();
                            if(wb)
                                wb.startDestroying();
                        }
                        finally { }
                    }
                }
                else if (collisionsGround.Length > 0)
                {
                    rb.AddForce(new Vector2(facingRight ? -30 : 30, 3), ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(new Vector2(facingRight ? -1 : 1, 3), ForceMode2D.Impulse);
                }
            }
        }
    }

    void resetMode()
    {
        cam.orthographicSize = defaultCamTransform;
        tr.startWidth = 1;
        sizeK = 1;
        trfm.localScale = new Vector3(defaultTransform.x * (facingRight ? 1 : -1), defaultTransform.y, defaultTransform.z);
        sizeMode = false;
    }

    public void switchMove(bool v)
    {
        canMove = v;
    }

    public void savePosition(Vector3 newSavedPosition)
    {
        savedPosition = newSavedPosition;
    }

    public void setCharges(int size = -1, int boost = -1, int leap = -1, int power = -1)
    {
        if (size > -1) sizeCharges = size;
        if (boost > -1) boostCharges = boost;
        if (leap > -1) leapCharges = leap;
        if (power > -1) powerCharges = power;
    }

    public void die()
    {
        canJump = false;
        canMove = false;
        setCharges(0, 0, 0, 0);
        Vector3 pos = trfm.position;
        Collider2D[] nearBlocksUp = Physics2D.OverlapBoxAll(new Vector3(pos.x, pos.y + 5, pos.z), new Vector2(5, 5), LayerMask.GetMask("Ground"));
        Collider2D[] nearBlocksLeft = Physics2D.OverlapBoxAll(new Vector3(pos.x - 5, pos.y, pos.z), new Vector2(5, 5), LayerMask.GetMask("Ground"));
        Collider2D[] nearBlocksDown = Physics2D.OverlapBoxAll(new Vector3(pos.x, pos.y - 5, pos.z), new Vector2(5, 5), LayerMask.GetMask("Ground"));
        Collider2D[] nearBlocksRight = Physics2D.OverlapBoxAll(new Vector3(pos.x + 5, pos.y, pos.z), new Vector2(5, 5), LayerMask.GetMask("Ground"));
        int lenUp = nearBlocksUp.Length;
        int lenLeft = nearBlocksLeft.Length;
        int lenDown = nearBlocksDown.Length;
        int lenRight = nearBlocksRight.Length;

        int max = Math.Max(Math.Max(lenUp, lenDown), Math.Max(lenLeft, lenRight));
        Collider2D[] nearBlocks = nearBlocksUp;

        if (max == lenLeft)
        {
            nearBlocks = nearBlocksLeft;
        }
        else if (max == lenDown)
        {
            nearBlocks = nearBlocksDown;
        }
        else if (max == lenRight)
        {
            nearBlocks = nearBlocksRight;
        }



        foreach (var block in nearBlocks)
        {
            block.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        }
    }
}
