using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicBlock : MonoBehaviour
{
    // DO NOT SUKA USE CHILDREN WITH THEIR OWN RIGIDBODIES!!!!!!!

    [HideInInspector] public enum AngleType { Up = 1, Left = 2, Down = 3, Right = 0};

    [HideInInspector] public AngleType typeOfAngle = AngleType.Up;

    [HideInInspector] public float delay = 1000f;
    [HideInInspector] public float expansion = 2f;
    [HideInInspector] public float expansionTime = 1000f;

    public bool reverseAtEnd = false;
    public bool cycle = false;
    public float delayAtEnd = 500f;

    public Transform[] startChain;
    public Transform[] endChain;

    private bool triggered = false;
    private bool finished = false;
    private float t;

    private Vector3 newPos, Pos;
    private int modX, modY = 0;

    private Rigidbody2D rb;
    private Transform trfm;
    private BoxCollider2D bc2d;

    private bool direct = true;

    // Use this for initialization
    void Start()
    {
        trfm = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        expansionTime += 1;
        expansionTime /= 1000;
        delay /= 1000;
        delayAtEnd /= 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && !finished)
        {
            //rb.AddForce(direction, ForceMode2D.Force);
            //rb.AddForce(direction*2);
            t += Time.deltaTime;
            rb.MovePosition(Vector2.Lerp(Pos, newPos, t / expansionTime));

            if (/*trfm.position == newPos && trfm.rotation.eulerAngles == newRot ||*/ t >= expansionTime)
            {
                finished = true;
                //bc2d.enabled = true;
                foreach (Transform t in endChain)
                {
                    try
                    {
                        StartCoroutine(t.GetComponent<KinematicBlock>().StartAnim(false));
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message + "\nError: " + t.name);
                    }
                }
                if (reverseAtEnd)
                {
                    t = 0;
                    if(typeOfAngle == AngleType.Up)
                    {
                        typeOfAngle = AngleType.Down;
                    }
                    else if (typeOfAngle == AngleType.Left)
                    {
                        typeOfAngle = AngleType.Right;
                    }
                    else if (typeOfAngle == AngleType.Down)
                    {
                        typeOfAngle = AngleType.Up;
                    }
                    else if(typeOfAngle == AngleType.Right)
                    {
                        typeOfAngle = AngleType.Left;
                    }

                    triggered = false;

                    if (direct)
                    {
                        StartCoroutine(StartAnim(true));
                    }
                    else if (cycle)
                    {
                        StartCoroutine(StartAnim(false));
                    }
                    direct = !direct;
                }
            }
        }
    }

    IEnumerator StartAnim(bool reverse)
    {
        
        if (!triggered)
        {
            bc2d.enabled = false;
            foreach (Transform t in startChain)
            {
                StartCoroutine(t.GetComponent<KinematicBlock>().StartAnim(reverse));
            }

            if (reverse)
            {
                Debug.Log("Started Reverse");
                yield return new WaitForSeconds(delayAtEnd);
            }
            else yield return new WaitForSeconds(delay);

            if (typeOfAngle == AngleType.Up)
            {
                modX = 0;
                modY = 1;
            }
            else if (typeOfAngle == AngleType.Left)
            {
                modX = -1;
                modY = 0;
            }
            else if (typeOfAngle == AngleType.Down)
            {
                modX = 0;
                modY = -1;
            }
            else if (typeOfAngle == AngleType.Right)
            {
                modX = 1;
                modY = 0;
            }
            Pos = trfm.position;
            newPos = new Vector3(Pos.x + modX * expansion, Pos.y + modY * expansion, Pos.z);

            triggered = true;
            finished = false;
        }
        else
        {
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(StartAnim(false));
        }
    }
}

