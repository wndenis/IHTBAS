using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraManager : MonoBehaviour {
    public static MainCameraManager mainCameraManager = null;
    void Awake()
    {
        if (mainCameraManager == null)
        {
            mainCameraManager = this;
        }
        else if (mainCameraManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }


    // Use this for initialization
    void Start () {
        GameUtils.gameUtils.cam = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
