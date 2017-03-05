using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugGUI : MonoBehaviour {
    public Transform player;
    private Controls c;
    public GUIStyle styleSize;
    public GUIStyle styleBoost;
    public GUIStyle styleLeap;
    public GUIStyle stylePower;

    // Use this for initialization
    void Start () {
        c = player.GetComponent<Controls>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 35, 25), ""+c.sizeCharges, styleSize);
        GUI.Label(new Rect(35, 5, 65, 25), ""+c.boostCharges, styleBoost);
        GUI.Label(new Rect(65, 5, 95, 25), ""+c.leapCharges, styleLeap);
        GUI.Label(new Rect(95, 5, 125, 25), ""+c.powerCharges, stylePower);
    }

}
