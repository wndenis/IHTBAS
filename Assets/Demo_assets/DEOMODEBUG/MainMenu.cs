using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    private bool CameraMoving = false;
    public Transform MainPos;
    public Vector3 MainPosV;

    public Transform StartPos;
    public Vector3 StartPosV;

    public Transform OptionsPos;
    public Vector3 OptionsPosV;

    public Transform CreditsPos;
    public Vector3 CreditsPosV;

    public Transform QuitPos;
    public Vector3 QuitPosV;

    // Use this for initialization
    void Start () {
        MainPosV = MainPos.position;
        StartPosV = StartPos.position;
        OptionsPosV = OptionsPos.position;
        CreditsPosV = CreditsPos.position;
        QuitPosV = QuitPos.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator MoveCamToPos(Vector3 position, float time)
    {
        float t = 0;
        while (GameUtils.gameUtils.camTrfm.position != position)
        {
            t += Time.deltaTime;
            GameUtils.gameUtils.camTrfm.position = Vector3.Lerp(
                GameUtils.gameUtils.camTrfm.position,
                position,
                t / time
                );
            yield return null;
        }
    }



    //Funcs================================

    public void Menu_Start()
    {
        StopAllCoroutines();
        StartCoroutine(DelayedStart(400));
    }
    private IEnumerator DelayedStart(float delay)
    {
        StartCoroutine(MoveCamToPos(StartPosV, delay));
        yield return StartCoroutine(CamUtils.FadeIn(3f));
        GameManager.gameManager.LoadScene("sandbox2");
    }

    public void Menu_Options()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCamToPos(OptionsPosV, 4));
    }

    public void Menu_Credits()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCamToPos(CreditsPosV, 4));
    }

    public void Menu_Quit()
    {
        StopAllCoroutines();
        StartCoroutine(DelayedQuit(400));
    }
    private IEnumerator DelayedQuit(float delay)
    {
        StartCoroutine(MoveCamToPos(QuitPosV, delay));
        yield return StartCoroutine(CamUtils.FadeIn(3));
        Application.Quit();
    }

    public void Menu_Back()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCamToPos(MainPosV, 8));
    }
}
