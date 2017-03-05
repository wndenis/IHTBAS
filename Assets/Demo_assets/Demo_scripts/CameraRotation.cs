using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {
    public Transform bg;
    private Transform tr;
    public bool rotate = true;

	// Use this for initialization
	void Start () {
        tr = GetComponent<Transform>();

    }
	
	// Update is called once per frame
	void Update () {
        if (rotate)
        {
            tr.Rotate(0, 0, -8 * Time.deltaTime);
            bg.Rotate(0, 0, -8 * Time.deltaTime);
        }
    }
}
