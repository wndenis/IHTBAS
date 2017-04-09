using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

    public float killDelay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(GameManager.gameManager.KillHeroWithDelay(killDelay + 5));
            StartCoroutine(GameManager.gameManager.StunHeroWithDelay(killDelay));
        }
    }
}
