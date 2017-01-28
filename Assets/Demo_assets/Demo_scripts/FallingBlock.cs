using UnityEngine;
using System.Collections;

public class FallingBlock : MonoBehaviour {

    IEnumerator startFall()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().isKinematic = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(startFall());
        }
    }


}
