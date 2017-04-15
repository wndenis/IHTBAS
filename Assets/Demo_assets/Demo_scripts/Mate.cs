using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mate : MonoBehaviour
{
    public Sprite chaseSprite;
    public Color endColor;
    public Transform particlesAtAnd;

    private Transform player;

    private Transform trfm;
    private Rigidbody2D rb;
    private TrailRenderer tr;
    private BoxCollider2D bc2d;
    private SpriteRenderer sr;

    private bool triggered;
    private bool finished;
    private bool bridging;
    private float selfDestructTime = 0f;

    private bool facingRight;
    private float t;

    private Vector3 oldPos;
    private Transform pointToMove;
    private Vector3 direction;
    private KinematicBlock foundTrap;
    private float speed;
    private float timeToBridge;

    // Use this for initialization
    void Start()
    {
        trfm = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        triggered = false;
        finished = false;
        bridging = false;
        facingRight = true;
        speed = Random.Range(0.8f, 3f);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(FadeIn());
        
        MatesManager.matesManager.AddMate(trfm);
    }

    // Update is called once per frame
    void Update() // MAYBE FIXEDUPDATE??????
    {
        if (!finished)
        {
            if (!triggered)
            {
                if (bridging)//=========Строим мост
                {
                    t += Time.deltaTime;
                    Debug.DrawLine(trfm.position, pointToMove.position, Color.white, 5f);
                    sr.color = Color.Lerp(sr.color, endColor, t / timeToBridge);
                    trfm.position = Vector2.Lerp(trfm.position, pointToMove.position, t / (timeToBridge*2));
                    trfm.localScale = Vector3.Lerp(trfm.localScale, Vector3.one, t / timeToBridge);
                    if ((Vector2)trfm.position == (Vector2)pointToMove.position)
                    {
                        finished = true;
                        gameObject.layer = LayerMask.NameToLayer("Ground"); // становимся землёй
                        Destroy(pointToMove.gameObject); // убиваем ненужный более поинт
                        rb.bodyType = RigidbodyType2D.Static; // оптимизируемся, становимся статик телом
                        bc2d.enabled = true; // включаем коллизии
                        if(selfDestructTime > 0f) // если нужно, то запускаем таймер самоуничтожения
                        {
                            StartCoroutine(SelfDestruct());
                        }
                    }
                }
                else //=================Просто плаваем за игроком
                {
                    CheckTraps();
                    t = 0;
                    Vector3 distance = (player.position - trfm.position).normalized * 3.6f;
                    if (distance.x < 0 && facingRight || distance.x > 0 && !facingRight)
                        Flip();
                    Vector3 playerPos = player.position;
                    playerPos.x += Random.Range(-1f, 1f);
                    playerPos.y += Random.Range(-1f, 1f);
                    trfm.position = Vector2.Lerp(trfm.position, player.position - distance, Time.deltaTime * speed);
                }
            }
            else if (triggered)//=========Блокируем ловушку
            {
                Debug.DrawLine(trfm.position, pointToMove.position, Color.yellow, 5f);
                t += Time.deltaTime;
                float timeCoefficient = Vector2.Distance(oldPos, pointToMove.position + direction) / speed;
                sr.color = Color.Lerp(sr.color, endColor, t / timeCoefficient);
                trfm.position = Vector3.Lerp(oldPos, pointToMove.position + direction, t / timeCoefficient);
                if (trfm.position == pointToMove.position + direction)
                {
                    finished = true;
                    foundTrap.StopAnim(); // останавливаем анимацию ловушки
                    GetComponent<SpriteRenderer>().color = endColor; // меняем цвет
                    rb.bodyType = RigidbodyType2D.Static; // становимся статичными для оптимизации
                    gameObject.layer = LayerMask.NameToLayer("Ground"); // становимся твёрдой землёй
                    Transform particles = Instantiate(particlesAtAnd); // выпускаем частицы
                    particles.position = new Vector3(trfm.position.x, trfm.position.y, particles.position.z); // корды частиц
                    var velocity = particles.GetComponent<ParticleSystem>().velocityOverLifetime; //-направление частиц
                    Vector2 velocityVector = (oldPos - trfm.position).normalized * -9f; //-----------------------------
                    velocity.x = velocityVector.x; // -----------------------------------------------------------------
                    velocity.y = velocityVector.y; // -----------------------------------------------------------------
                    StartCoroutine(DisableTrail()); // отключаем хвост через несколько секунд
                    StartCoroutine(CamUtils.Shake(0.15f, 0.07f)); // Трясем экран
                    StartCoroutine(fixPosition()); // подгоняем положение
                }
            }
        }
    }

    IEnumerator fixPosition()
    {
        yield return new WaitForSeconds(0.05f);
        trfm.position = pointToMove.position + direction;
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructTime);
        Transform particles = Instantiate(particlesAtAnd); // выпускаем частицы
        particles.position = new Vector3(trfm.position.x, trfm.position.y, particles.position.z); // корды частиц
        Destroy(gameObject);
    }

    IEnumerator FadeIn()
    {
        Color newColor = sr.color;
        Color oldColor = new Color(newColor.r, newColor.g, newColor.b, 0);
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
        {
            sr.color = Color.Lerp(oldColor, newColor, t);
            yield return null;
        }
    }

    public bool isFree()
    {
        return !(triggered && finished && bridging);
    }

    public void SetPointToMoveBridging(Transform point, float timeToBridge, float selfDestructTime = 0f)
    {
        bc2d.enabled = false;
        bridging = true;
        pointToMove = point;
        this.selfDestructTime = selfDestructTime;
        this.timeToBridge = timeToBridge;
        this.oldPos = trfm.position;
        t = 0;
    }

    private IEnumerator DisableTrail()
    {
        yield return new WaitForSeconds(2);
        Destroy(tr);
    }

    void Flip() // flipping player's view
    {
        trfm.localScale = new Vector3(trfm.localScale.x * -1, trfm.localScale.y, trfm.localScale.z);
        facingRight = !facingRight;
    }

    void CheckTraps()
    {
        Collider2D[] nearestTraps = Physics2D.OverlapCircleAll(player.position, 4f, LayerMask.GetMask("Traps"));
        if (nearestTraps != null && nearestTraps.Length > 0)
        {
            foreach (Collider2D trap in nearestTraps)
            {
                foundTrap = trap.GetComponent<KinematicBlock>();

                if (foundTrap.isTriggered()) // Если ловушка активирована и ее никто не собирается останавливать
                {
                    // определяем вектор движения ловушки
                    direction = foundTrap.getVectorAngle();
                    
                    // Ищем блок фронта движения
                    Transform nearestTrapBlock = trap.transform.GetChild(0);
                    /*
                    foreach (Transform trapBlock in trap.transform)
                    {
                        if (trapBlock.position.x * direction.x >= nearestTrapBlock.position.x * direction.x)
                            if(trapBlock.position.y * direction.y >= nearestTrapBlock.position.y * direction.y)
                                nearestTrapBlock = trapBlock;
                    }*/
                    // запрещаем другим блокировать эту ловушку
                    foundTrap.Block();

                    // вычисляем скорость движения
                    float trapSpeed = foundTrap.expansion / (foundTrap.expansionTime);
                    speed = trapSpeed * 20;

                    // сбрасываем время анимации
                    t = 0;

                    // переходим в режим ускорения
                    triggered = true;

                    // меняем свой спрайт
                    GetComponent<SpriteRenderer>().sprite = chaseSprite;
                    trfm.localScale = Vector3.one;

                    // включаем хвост
                    tr.time = 1f;

                    // летим на точку и отключаемся
                    pointToMove = nearestTrapBlock;
                    
                    //сохраняем старое положение для интерполяции
                    oldPos = trfm.position;

                    // удаляем себя из менеджера, чтобы больше нас не трогал
                    MatesManager.matesManager.RemoveMate(trfm);
                    break;
                }
            }
        }
    }
}
