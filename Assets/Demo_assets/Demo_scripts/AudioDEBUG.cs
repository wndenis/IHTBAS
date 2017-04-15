using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDEBUG : MonoBehaviour {
    private AudioSource aus;

	// Use this for initialization
	void Start () {
        aus = GetComponent<AudioSource>();
        aus.time = 58;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
