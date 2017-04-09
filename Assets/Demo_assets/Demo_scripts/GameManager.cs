using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager = null;
    public Camera cam = null;
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


    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Single);
        hero.transform.position = hControls.savedPosition;
        if (!hControls.facingRight)
        {
            hControls.Flip();
        }
        hControls.Start();
    }

    public IEnumerator StunHeroWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.hControls.SetStun(false);
        yield return null;
    }

    public IEnumerator KillHeroWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.RestartLevel();
        yield return null;
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
            RestartLevel();
        }

    }


}

