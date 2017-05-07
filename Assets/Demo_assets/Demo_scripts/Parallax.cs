using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    public bool parallax;
    public float parallaxSpeedX;
    public float parallaxSpeedY;

    private Transform trfm;
    private float lastCamX, lastCamY;
    private Transform cameraTransform;

    /*
    private Transform[] layers;
    private float viewZone = 10;
    private int leftIndex;
    private int rightIndex;*/

    
    
    private void Start()
    {
        trfm = transform;
        cameraTransform = GameUtils.gameUtils.camTrfm;
        lastCamX = cameraTransform.position.x;
        lastCamY = cameraTransform.position.y;
        /*
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);
        leftIndex = 0;
        rightIndex = layers.Length - 1;*/
    }

    private void Update()
    {
        if (parallax)
        {
            float dx = cameraTransform.position.x - lastCamX;
            float dy = cameraTransform.position.y - lastCamY;
            transform.position += Vector3.right * (dx * parallaxSpeedX);
            transform.position += Vector3.up * (dy * parallaxSpeedY);

            lastCamX = cameraTransform.position.x;
            lastCamY = cameraTransform.position.y;
        }

    }
}
