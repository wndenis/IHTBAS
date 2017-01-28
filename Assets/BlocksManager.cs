using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BlocksManager : MonoBehaviour
{
    public GameObject block;

    private static Vector3 target;

    private float height;
    private float width;

    private Transform t;

    [HideInInspector]
    public Transform player;
    private bool trig;
    private bool wasTrig;
    private GameObject chaser;



    void Start()
    {
        player = FindObjectOfType<Controls>().transform;
        t = transform;

        height = block.GetComponent<BlocksBeh>().getHeight();
        width = block.GetComponent<BlocksBeh>().getWidth();
        target = new Vector3(nearestMul(t.position.x, width / 4), nearestMul(t.position.y, height / 4), t.position.z - 1);
        chaser = (GameObject)Instantiate(block, target, Quaternion.identity, transform);

        //Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        
    }
    /*
    public static void sortBlocks()
    {
        blocks.Sort(delegate (BlocksBeh a, BlocksBeh b)
        {
            try
            {
                return a.Distance(target).CompareTo(b.Distance(target));
            }
            catch(System.Exception e)
            {
                return 0;
            }
        }
            );
    }*/
    

    // Update is called once per frame
    void FixedUpdate()
    {
        chaser.GetComponent<BlocksBeh>().PathFind(player.position);
    }


    public static float nearestMul(float num, float mul)
    {
        return Mathf.Round(num / mul) * mul;
    }



    /*
    static bool checkXY(float x, float y, float w, float h)
    {
        x = nearestMul(x, w);
        y = nearestMul(y, h);
        Debug.Log("X: " + x + "  Y: " + y);
        foreach (BlocksBeh b in blocks)
        {
            if ((nearestMul(b.transform.position.y, h) == y) && (nearestMul(b.transform.position.x, w) == x)) { Debug.Log("False"); return false; }
        }

        return true;
    }

    public static Vector3 getNextTarget(BlocksBeh nearest, Vector3 targett)
    {
        Vector3 target = new Vector3(nearest.t.position.x, nearest.t.position.y, 0);

        float w = nearest.getWidth();
        float h = nearest.getHeight();


        List<int> actions = new List<int>() { 1, 2, 3, 4 };
        int n = actions.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int tmp = actions[k];
            actions[k] = actions[n];
            actions[n] = tmp;
        }
        bool actFound = false;
        foreach (int act in actions)
        {
            switch (act)
            {
                case 1:
                    {
                        if (checkXY(target.x + nearest.getWidth(), target.y, w, h))
                        {
                            actFound = true;
                            target.x += nearest.getWidth();
                        }
                        break;
                    }
                case 2:
                    {
                        if (checkXY(target.x - nearest.getWidth(), target.y, w, h))
                        {
                            actFound = true;
                            target.x -= nearest.getWidth();
                        }
                        break;
                    }
                case 3:
                    {
                        if (checkXY(target.x, target.y + nearest.getHeight(), w, h))
                        {
                            actFound = true;
                            target.y += nearest.getHeight();
                        }
                        break;
                    }
                case 4:
                    {
                        if (checkXY(target.x, target.y - nearest.getHeight(), w, h))
                        {
                            actFound = true;
                            target.y -= nearest.getHeight();
                        }
                        break;
                    }
            }
            if (actFound) break;
        }
        if (!actFound) { throw new System.Exception("некуда идти"); }
        return target;
    }


    public static Vector3 getNextTarget(List<BlocksBeh> c, Vector3 targett)
    {
        return getNextTarget(c[0], targett);
    }


    public void ChainMove(Vector3 tr)
    {
        if (tr != target)
        {
            //Debug.Log("STARTTT");
            ChainMove();
        }
    }

    public void ChainMove()
    {
        if (!(target.Equals(null)))
        {
            sortBlocks();
            try
            {
                Vector3 tg = getNextTarget(blocks, target);
                blocks[blocks.Count - 1].PathFind(tg);
                //blocks[blocks.Count - 1].PathFind(tg);
            }
            catch(System.Exception e)
            {

            }
        }
    }*/
}