using UnityEngine;
using System.Collections;

public class Hurt : MonoBehaviour
{
    private Controls player;
    public Transform start;

    void Start()
    {
        player = FindObjectOfType<Controls>();
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.rb.velocity = new Vector2(0, 0);
            player.transform.position = start.position;
        }
    }
}
