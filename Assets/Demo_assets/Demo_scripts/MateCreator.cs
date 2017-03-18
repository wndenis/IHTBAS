using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateCreator : MonoBehaviour {
    public int count;
    public Transform mate;

    private bool triggered = false;
    private Transform trfm;

	// Use this for initialization
	void Start () {
        trfm = GetComponent<Transform>();
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
                triggered = true;
                for (int i = 0; i < count; i++)
                {
                    //Instantiate(mate).position = new Vector3(trfm.position.x + Random.Range(-1, 1), trfm.position.y + Random.Range(-1, 1), mate.position.z);
                    Transform newMate = Instantiate(mate);
                    //print("Old: " + newMate.position);
                    newMate.position = (Vector3)Random.insideUnitCircle + trfm.position;
                    //print("New: " + newMate.position);
                }
            }
        }
    }
}
