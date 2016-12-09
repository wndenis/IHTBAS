using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{
    public float amounttomovex;
    public float speed;
    private float currentposx;
    private float currentposy;
    private int facing;
    private Controls player;
    public Transform start;
    private SpriteRenderer sr;

    void Start()
    {
        currentposx = gameObject.transform.position.x;
        facing = 0;
        player = FindObjectOfType<Controls>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (facing == 1 && gameObject.transform.position.x < currentposx - amounttomovex)
        {
            facing = 0;
        }

        if (facing == 0 && gameObject.transform.position.x > currentposx)
        {
            facing = 1;
        }

        if (facing == 0)
        {
            sr.flipX = false;
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (facing == 1)
        {
            sr.flipX = true;
            transform.Translate(-Vector2.right * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(player.rb.velocity.y < -3.5)
            {
                sr.color *= 0;
                Destroy(this);
            }
            else
            {
                player.rb.velocity = new Vector2(0, 0);
                player.transform.position = start.position;
            }
        }
    }



}
