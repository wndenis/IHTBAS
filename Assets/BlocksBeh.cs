using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BlocksBeh : MonoBehaviour
{
    public GameObject pathTile;
    private List<Object> objs;


    public float speed = 35;
    public float bonus = 0f;//0.15f;

    private Vector3 vector;
    private Vector3 target;
    [HideInInspector]
    public Transform t;
    private List<Vector3> way;

    private float height;
    private float width;

    private bool moving;

    public List<BlocksBeh> chain;
    //public Vector3 mainTarget;


    // Use this for initialization
    void Start()
    {
        t = GetComponent<Transform>();
        way = new List<Vector3>();
        objs = new List<Object>();
        chain = new List<BlocksBeh>();
        moving = false;
        height = getHeight();
        width = getWidth();
    }

    public void setVector(Vector3 v)
    {
        vector = v;
    }
    /*
    public void setChain(List<BlocksBeh> c)
    {
        chain = c;
    }
    
    public void setMainTarget(Vector3 v)
    {
        mainTarget = v;
    }
    */
    public float getWidth()
    {
        return Utils.getGameObjectWidth(gameObject);
    }

    public float getHeight()
    {
        return Utils.getGameObjectWidth(gameObject);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        height = getHeight();
        width = getWidth(); 

        if (t.position != vector && moving)
        {
            t.position = Vector3.Lerp(t.position, vector, speed * Time.fixedDeltaTime + bonus);
        }
        else
        {
            
            if (way.Count > 0)
            {
                vector = way[0];
                way.RemoveAt(0);
                //Instantiate(pathTile, way[1], Quaternion.identity, transform.parent);
                //Destroy(objs[0]);
                //objs.RemoveAt(0);
            }
            else
            {
                if (moving)
                {
                    moving = false;
                    
                    //target = t.parent.GetComponent<BlocksManager>().player.position;
                    //PathFind(target);
                    //t.parent.GetComponent<BlocksManager>().ChainMove(target);

                }
            }
        }

    }

    bool absDif(float a, float b)
    {
        float dif = 0.08f;
        return !(Mathf.Abs(a - b) < dif);
    }

    public float Distance(Vector3 target)
    {
        return Vector3.Distance(target, t.position);
    }

    public void PathFind(Vector3 targett)
    {
        target = targett;
        if (moving) {
            way = new List<Vector3>() { way[0] };
        }
        else
        {
            way = new List<Vector3>();
        }
        /*foreach (Object o in objs)
        {
            Destroy(o);
            objs.Remove(o);
        }*/

        List<int> actions = new List<int>() {1, 2, 3, 4 };

        target.x = BlocksManager.nearestMul(target.x, width);
        target.y = BlocksManager.nearestMul(target.y, height);

        

        bool wayBuilding = true;

        float stepsX = 0;
        float stepsY = 0;

        int i = 0;

        while (wayBuilding)
        {
            i++;
            if (i > 150)
            {
                //Debug.Log("Fail");
                break;
            }

            // shuffle
            int n = actions.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                int tmp = actions[k];
                actions[k] = actions[n];
                actions[n] = tmp;
            }

            float tx = transform.position.x + width * stepsX;
            float ty = transform.position.y + height * stepsY;

            bool actFound = false;
            foreach(int act in actions)
            {
                switch (act)
                {
                    case 1:
                        {
                            if (absDif(tx, target.x) && tx > target.x)
                            {
                                actFound = true;
                                stepsX--;
                            }
                            
                            break;
                        }
                    case 2:
                        {
                            if (absDif(ty, target.y) && ty < target.y)
                            {
                                actFound = true;
                                stepsY++;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (absDif(tx, target.x) && tx < target.x)
                            {
                                actFound = true;
                                stepsX++;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (absDif(ty, target.y) && ty > target.y)
                            {
                                actFound = true;
                                stepsY--;
                            }
                            break;
                        }
                }
                if (actFound) break;
            }

            Vector3 point = new Vector3(transform.position.x + width * stepsX, transform.position.y + height * stepsY, transform.position.z);
            way.Add(point);
            //objs.Add(Instantiate(pathTile, new Vector3(point.x, point.y, 5), Quaternion.identity, transform.parent));

            if ((Mathf.Abs(point.x - target.x) < 0.05) && (Mathf.Abs(point.y - target.y) < 0.05))
            {
                vector = way[0];
                moving = true;
                break;
            }

        }
        
        return;//yield break;
    }
}