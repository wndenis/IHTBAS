using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils : MonoBehaviour
{
    public Camera cam = null;
    public Transform hero = null;


    public static GameUtils gameUtils = null;

    void Awake()
    {
        if (gameUtils == null)
            gameUtils = this;

        else if (gameUtils != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        if (hero != null)
        {
            Debug.Log("Герой задан");
            hRigidbody2D = hero.GetComponent<Rigidbody2D>();
            hSpriteRenderer = hero.GetComponent<SpriteRenderer>();
            hControls = hero.GetComponent<Controls>();
            hCircleCollider2D = hero.GetComponent<CircleCollider2D>();
            hTrailRenderer = hero.GetComponent<TrailRenderer>();
            hAnimator = hero.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("Герой не задан!");
        }

        if (cam != null)
        {
            Debug.Log("Камера задана");
            camTrfm = cam.GetComponent<Transform>();
            dGUI = cam.GetComponent<debugGUI>();
        }
        else
        {
            Debug.LogWarning("Камера не задана!");
        }
    }


    // CAM COMPONENTS
    public Transform camTrfm;
    public debugGUI dGUI;
    // CAM COMPONENTS.

    // HERO COMPONENTS
    public Rigidbody2D hRigidbody2D;
    public SpriteRenderer hSpriteRenderer;
    public Controls hControls;
    public CircleCollider2D hCircleCollider2D;
    public TrailRenderer hTrailRenderer;
    public Animator hAnimator;
    // HERO COMPONENTS.

}

