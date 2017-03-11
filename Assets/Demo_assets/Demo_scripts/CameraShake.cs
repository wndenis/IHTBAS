using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private void Start()
    {

    }

    public void StartShake(float duration, float magnitude)
    {
        StopCoroutine("Shake");
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

            transform.position += new Vector3(x, y, 0);

            yield return null;
        }

        //trfm.position = originalCamPos;
    }
}
