using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakBlock : MonoBehaviour {
    private ParticleSystem ps;
    private SpriteRenderer sr;
    private BoxCollider2D bc2d;
    private Vector3 startPos;
    private Transform trfm;
    public Transform particles;

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
            startDestroying(Vector2.zero);
        }
	}

    public void startDestroying(Vector2 direction)
    {
        Transform particlesT = Instantiate(particles);
        particlesT.position = trfm.position;
        ps = particlesT.GetComponent<ParticleSystem>();
        var psVelocity = ps.velocityOverLifetime;
        psVelocity.x = direction.x;
        psVelocity.y = direction.y;
        sr = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        bc2d.enabled = false;
        sr.color = Color.clear;
    }
}
