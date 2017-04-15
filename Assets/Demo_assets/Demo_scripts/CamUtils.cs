using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamUtils : MonoBehaviour {
    public static IEnumerator FadeIn(float time)
    {
        debugGUI dGUI = GameUtils.gameUtils.dGUI;
        float alphaFade = 0;
        dGUI.MakeFader();
        for (float t = 0.0f; alphaFade != 1; t += Time.deltaTime)
        {
            alphaFade = Mathf.Clamp01(t / time);
            dGUI.fader.color = new Color(0, 0, 0, alphaFade);
            yield return null;
        }
    }

    public static IEnumerator FadeOut(float time)
    {
        debugGUI dGUI = GameUtils.gameUtils.dGUI;
        float alphaFade = 1;
        for (float t = 0.0f; alphaFade != 0; t += Time.deltaTime)
        {
            alphaFade = Mathf.Clamp01(1 - t / time);
            dGUI.fader.color = new Color(0, 0, 0, alphaFade);
            yield return null;
        }
        Destroy(dGUI.fader.gameObject);
    }

    //===========================CAMSHAKING
    public void StopShake()
    {
        StopAllCoroutines();
    }

    public static IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        Transform cam = GameUtils.gameUtils.camTrfm;
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
}
