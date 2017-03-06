using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicBlock : MonoBehaviour
{

    [HideInInspector] public enum AngleType { Up = 1, Left = 2, Down = 3, Right = 0, Custom = -1 };

    [HideInInspector] public AngleType typeOfAngle = AngleType.Up;
    [HideInInspector] public float angle;

    [HideInInspector] public float rotation;

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
    private Vector3 newPos, Pos, newRot, Rot;

    private Transform trfm;

    private bool direct = true;

    private int modX, modY = 0;

    // Use this for initialization
    void Start()
    {
        trfm = transform;
        expansionTime += 1;
        expansionTime /= 1000;
        delay /= 1000;
        delayAtEnd /= 1000;
        /*
        if(typeOfAngle != AngleType.Custom)
        {
            angle = (float)typeOfAngle * Mathf.PI / 2;
        }

        newPos = new Vector3(Pos.x + expansion * Mathf.Cos(angle), Pos.y + expansion * Mathf.Sin(angle), Pos.z);
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && !finished)
        {
            t += Time.deltaTime;
            //transform.localScale = Vector3.Lerp(oldScale, scale, t / expansionTime);
            trfm.position = Vector3.Lerp(Pos, newPos, t / expansionTime);
            trfm.Rotate(0, 0, newRot.z / expansionTime * Time.deltaTime);
            //trfm.rotation = Vector3.Lerp(Rot, newRot, t/expansionTime);

            if (/*trfm.position == newPos && trfm.rotation.eulerAngles == newRot ||*/ t >= expansionTime)
            {
                finished = true;
                foreach (Transform t in endChain)
                {
                    try
                    {
                        StartCoroutine(t.GetComponent<KinematicBlock>().StartAnim(false));
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error: " + t.name);
                    }
                }
                if (reverseAtEnd)
                {
                    t = 0;
                    Vector3 tmp = Pos;
                    Pos = newPos;
                    newPos = tmp;

                    tmp = Rot;
                    Rot = newRot;
                    newRot = tmp;

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
        /*else if (triggered && finished)
        {

        }*/
    }

    IEnumerator StartAnim(bool reverse)
    {
        if (!triggered)
        {
            for (int i = 0; i < trfm.childCount; i++)
            {
                try
                {
                    trfm.GetChild(i).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                }
                catch (Exception e)
                {

                }
            }
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

            modX = 0;
            modY = 1;

            if (typeOfAngle == AngleType.Left)
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
            Rot = trfm.rotation.eulerAngles;

            newPos = new Vector3(Pos.x + modX * expansion, Pos.y + modY * expansion, Pos.z);
            newRot = new Vector3(Rot.x, Rot.y, rotation * 360);

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

