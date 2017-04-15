using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCameraShaker : MonoBehaviour {
    public bool enable;
    public float duration;
    public float magnitude;

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
            if (enable)
                StartCoroutine(CamUtils.Shake(duration, magnitude));
            else { }
               // CamUtils.StopShake();
        }
    }
}
