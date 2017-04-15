using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager = null;

    void Awake()
    {
        if (gameManager == null)
            gameManager = this;

        else if (gameManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }


    public void LoadScene(string name)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(name);
        Destroy(GameUtils.gameUtils.gameObject);
    }

    public IEnumerator RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        StartCoroutine(StunHeroWithDelay(0.5f, true));
        yield return StartCoroutine(CamUtils.FadeIn(1f)); // затемняем экран

        // затем перезагружаем уровень
        SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Single);
        GameUtils.gameUtils.hero.position = GameUtils.gameUtils.hControls.savedPosition;
        GameUtils.gameUtils.camTrfm.position = GameUtils.gameUtils.hero.position;
        if (!GameUtils.gameUtils.hControls.facingRight)
        {
            GameUtils.gameUtils.hControls.Flip();
        }
        GameUtils.gameUtils.hControls.Start();
        StartCoroutine(StunHeroWithDelay(1f, false));
        StartCoroutine(CamUtils.FadeOut(1f));
        yield return null;
    }

    public IEnumerator StunHeroWithDelay(float delay, bool state)
    {
        yield return new WaitForSeconds(delay);
        GameUtils.gameUtils.hControls.SetStun(state);
        yield return null;
    }

    public IEnumerator KillHeroWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(gameManager.RestartLevel());
        yield return null;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RestartLevel());
        }

    }


}

