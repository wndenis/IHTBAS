using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugGUI : MonoBehaviour {
    //private Transform hero;

    public GUIStyle styleSize;
    public GUIStyle styleBoost;
    public GUIStyle styleLeap;
    public GUIStyle stylePower;

    public Transform Fader;
    public SpriteRenderer fader;

    // Use this for initialization
    void Start () {
        MakeFader();
        StartCoroutine(CamUtils.FadeOut(1));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MakeFader()
    {
        fader = Instantiate(Fader, GameUtils.gameUtils.camTrfm).GetComponent<SpriteRenderer>();
    }

    private void OnGUI()
    {
        //GUI.Label(new Rect(5, 5, 35, 25), ""+c.sizeCharges, styleSize);
        if (GameUtils.gameUtils.hControls != null)
        {
            GUI.Label(new Rect(35, 5, 65, 25), "" + GameUtils.gameUtils.hControls.boostCharges, styleBoost);
            GUI.Label(new Rect(65, 5, 95, 25), "" + GameUtils.gameUtils.hControls.leapCharges, styleLeap);
            GUI.Label(new Rect(95, 5, 125, 25), "" + GameUtils.gameUtils.hControls.powerCharges, stylePower);
        }
    }
}
