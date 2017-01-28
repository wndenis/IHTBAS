using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Hurt : MonoBehaviour
{
    private Controls player;
    public Transform start;

    void Start()
    {
        player = FindObjectOfType<Controls>();
    }


    void Die()
    {
        player.switchMove(false);
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.rb.velocity = new Vector2(0, 0);
            Die();
            //player.transform.position = start.position;
        }
    }
}
