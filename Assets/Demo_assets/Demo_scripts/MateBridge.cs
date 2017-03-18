using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateBridge : MonoBehaviour {
    public float delay = 0.1f;
    public float timeToMove = 0.5f;
    public float selfDestructTime = 0f;

    private Transform trfm;
    private bool triggered = false;
	// Use this for initialization
	void Start () {
        trfm = GetComponent<Transform>();
        BoxCollider2D bc2d = GetComponent<BoxCollider2D>();
        bc2d.offset += new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.4f, 0.4f));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            if (collision.tag == "Player")
            {
                if (trfm.childCount == 0)
                {
                    triggered = true;
                    return;
                }
                    
                Transform[] children = new Transform[trfm.childCount];
                int i = 0;
                foreach (Transform child in trfm)
                {
                    children[i++] = child;
                }

                MatesManager.matesManager.OrganizeBridge(children, delay, timeToMove, selfDestructTime);
            }
        }
    }
}
