using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPostProcess : MonoBehaviour {
    public float frequency;
    private float timeFromLast = 0;
    private float newAlpha;
    public float min;
    public float max;

    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        min = min / 255;
        max = max / 255;
        StartCoroutine(Animate());
	}

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator Animate()
    {
        timeFromLast = 0;
        newAlpha = Random.Range(min, max);
        float oldAlpha = sr.color.a;
        while (timeFromLast < frequency)
        {
            timeFromLast += Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(oldAlpha, newAlpha, timeFromLast/frequency));
            yield return null;
        }
        StartCoroutine(Animate());
    }


}
