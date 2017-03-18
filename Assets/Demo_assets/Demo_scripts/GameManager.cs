using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager = null;
    public Camera cam;

    void Awake()
    {
        if (gameManager == null)
            gameManager = this;

        else if (gameManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        cam = Camera.main;
    }

    void Start()
    {
        
    }

    //===========================CAMSHAKING
    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    public void StopShake()
    {
        StopCoroutine("Shake");
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;

           cam.transform.position += new Vector3(x, y, 0);

            yield return null;
        }

        //trfm.position = originalCamPos;
    }
    //===================================================================





    // Update is called once per frame
    void Update()
    {

    }




}

