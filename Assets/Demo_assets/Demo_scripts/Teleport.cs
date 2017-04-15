using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    public Transform Point;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector3 delta = GameUtils.gameUtils.cam.transform.position - collision.transform.position;
            collision.transform.position = Point.position;
            GameUtils.gameUtils.cam.transform.position = Point.position + delta;
        }
    }
}
