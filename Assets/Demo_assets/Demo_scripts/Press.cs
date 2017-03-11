using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : MonoBehaviour {
    public float delay = 1000f;
    public float expansion = 2f;
    public float expansionTime = 1000f;
    public Transform[] startChain;
    public Transform[] endChain;


    private bool triggered = false;
    private bool finished = false;
    private float t;
    private Vector3 scale, oldScale, pos, oldPos;
    //private Animator anim;



    // Use this for initialization
    void Start () {
        //anim = GetComponent<Animator>();
        expansionTime /= 1000;
        delay /= 1000;

        oldScale = transform.localScale;
        scale = new Vector3(oldScale.x, oldScale.y * (expansion + 1), oldScale.z);

        oldPos = transform.position;

        float xmod = 0, ymod = 0;
        float rot = transform.rotation.eulerAngles.z;
        if (rot == 0) { xmod = 0; ymod = 1;}
        else if (rot == 90) { xmod = -1; ymod = 0;}
        else if (rot == 180) { xmod = 0; ymod = -1;}
        else if (rot == 270) { xmod = 1; ymod = 0;}

        xmod *= expansion / 2;
        ymod *= expansion / 2;

        pos = new Vector3(oldPos.x + xmod, oldPos.y + ymod, oldPos.z);
    }
	
	// Update is called once per frame
	void Update () {
		if(triggered && !finished)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(oldScale, scale, t / expansionTime);
            transform.position = Vector3.Lerp(oldPos, pos, t / expansionTime);
            if(transform.position == pos)
            {
                finished = true;
                foreach (Transform t in endChain)
                {
                    try
                    {
                        StartCoroutine(t.GetComponent<Press>().StartAnim());
                    }
                    finally{/*Debug.Log("Error: " + t.name);*/}
                }
            }
        }
        else if(triggered && finished)
        {
            
        }
	}

    IEnumerator StartAnim()
    {
        if (!triggered)
        {
            yield return new WaitForSeconds(delay);
            triggered = true;
        }
        else
        {
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(StartAnim());
            foreach(Transform t in startChain)
            {
                StartCoroutine(t.GetComponent<Press>().StartAnim());
            }
        }
    }
}
