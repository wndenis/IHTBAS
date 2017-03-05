using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePod : MonoBehaviour {
    // -1 means no correction (leaves charge as it)
    [Range(-1, 10)] public int sizeCharge = -1;
    [Range(-1, 10)] public int boostCharge = -1;
    [Range(-1, 10)] public int leapCharge = -1;
    [Range(-1, 10)] public int powerCharge = -1;
    private ParticleSystem ps;

	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Controls>().setCharges(sizeCharge, boostCharge, leapCharge, powerCharge);
            ps.Play();
        }
    }
}
