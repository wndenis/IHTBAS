using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager = null;
    public Camera cam = null;
    public Transform camTrfm;
    public debugGUI dGUI;
    public Transform hero;


    // COMPONENTS
    public Rigidbody2D hRigidbody2D;
    public SpriteRenderer hSpriteRenderer;
    public Controls hControls;
    public CircleCollider2D hCircleCollider2D;
    public TrailRenderer hTrailRenderer;
    public Animator hAnimator;

    // COMPONENTS.




    void Awake()
    {
        if (gameManager == null)
            gameManager = this;

        else if (gameManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        camTrfm = cam.transform;
        dGUI = cam.GetComponent<debugGUI>();

        hRigidbody2D = hero.GetComponent<Rigidbody2D>();
        hSpriteRenderer = hero.GetComponent<SpriteRenderer>();
        hControls = hero.GetComponent<Controls>();
        hCircleCollider2D = hero.GetComponent<CircleCollider2D>();
        hTrailRenderer = hero.GetComponent<TrailRenderer>();
        hAnimator = hero.GetComponent<Animator>();
    }

    private void Start()
    {
        
    }


    public IEnumerator RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        StartCoroutine(StunHeroWithDelay(0.5f, true));
        yield return StartCoroutine(FadeIn(1f)); // затемняем экран

        // затем перезагружаем уровень
        SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Single);
        hero.position = hControls.savedPosition;
        camTrfm.position = hero.position;
        if (!hControls.facingRight)
        {
            hControls.Flip();
        }
        hControls.Start();
        StartCoroutine(StunHeroWithDelay(1f, false));
        StartCoroutine(FadeOut(1f));
        yield return null;
    }

    public IEnumerator StunHeroWithDelay(float delay, bool state)
    {
        yield return new WaitForSeconds(delay);
        gameManager.hControls.SetStun(state);
        yield return null;
    }

    public IEnumerator KillHeroWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(gameManager.RestartLevel());
        yield return null;
    }

    public IEnumerator FadeIn(float time)
    {
        float alphaFade = 0;
        dGUI.MakeFader();
        for (float t = 0.0f; alphaFade != 1; t += Time.deltaTime)
        {
            alphaFade = Mathf.Clamp01(t / time);
            dGUI.fader.color = new Color(0, 0, 0, alphaFade);
            yield return null;
        }
    }

    public IEnumerator FadeOut(float time)
    {
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RestartLevel());
        }

    }


}

