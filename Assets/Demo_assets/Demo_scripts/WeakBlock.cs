using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakBlock : MonoBehaviour {
    private ParticleSystem ps;
    private SpriteRenderer sr;
    private BoxCollider2D bc2d;
    private Vector3 startPos;
    private Transform trfm;

	// Use this for initialization
	void Start () {
        trfm = GetComponent<Transform>();
        startPos = trfm.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (ps)
        {
            if (ps.isStopped)
            {
                Destroy(gameObject);
            }
        }
        else if (startPos != trfm.position)
        {
            startDestroying();
        }
	}

    public void startDestroying()
    {
        ps = GetComponent<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();

        bc2d.enabled = false;
        ps.Play();
        sr.color = new Color(0, 0, 0, 0);
    }
}
