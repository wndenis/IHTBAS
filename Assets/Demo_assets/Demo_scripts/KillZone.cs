using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {
    [HideInInspector]
    public float killDelay;
    [HideInInspector]
    public bool triggered = false;
    [HideInInspector]
    public KinematicBlock kinematic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggered = true;
            kinematic.GentleStop();
            StartCoroutine(GameManager.gameManager.KillHeroWithDelay(killDelay + 2.5f));
            StartCoroutine(GameManager.gameManager.StunHeroWithDelay(killDelay, true));
        }
    }
}
