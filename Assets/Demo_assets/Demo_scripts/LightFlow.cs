using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlow : MonoBehaviour {
    /// <summary>
    /// Слои, с которыми идет работа рейкастинга
    /// </summary>
    public LayerMask layers;
    
    public float distance = 100;
    [Range(0,65)]
    public int bounces = 1;

    public Transform[] areas;
    public int[] reflectionsForAreas;
    private SpriteRenderer[] lightAreas; // спрайты
    private BoxCollider2D[] colliderAreas; // их коллайдеры

    public Transform lightPrefab;

    private List<Transform> lights;
    private LineRenderer line;
    private List<Vector3> points;
    private RaycastHit2D[] hits;

    // Use this for initialization
    void Start () {
        line = GetComponent<LineRenderer>();
        points = new List<Vector3>();
        lights = new List<Transform>();
        Physics2D.queriesStartInColliders = false;

        // сразу получаем SpriteRenderer от каждой зоны, чтобы потом не вызывать ее по сто раз
        // Заодно записываем уже цвета, чтобы упростить код
        lightAreas = new SpriteRenderer[areas.Length];
        for(int i = 0; i < areas.Length; i++)
        {
            lightAreas[i] = areas[i].GetComponent<SpriteRenderer>();
        }

        colliderAreas = new BoxCollider2D[areas.Length];
        for (int i = 0; i < areas.Length; i++)
        {
            colliderAreas[i] = areas[i].GetComponent<BoxCollider2D>();
        }
    }

    // Update is called once per frame
    void Update () {
        drawRay();
	}


    void drawRay()
    {
        /*
        for(int j = 0; j < lights.Count; j++)
        {
            //print("L:" + lights[j].position + " P:" + points[j]);
            if (lights[j].position.x != points[j].x || lights[j].position.y != points[j].y)
            {
                for(int k = j; k < lights.Count; k++)
                    Destroy(lights[k].gameObject);
                //print("Destroyed " + (lights.Count - j) + "/" + lights.Count + " objects");
                lights.RemoveRange(j, lights.Count - j);
                break;
            }
        }*/

        Ray2D ray = new Ray2D(transform.position, transform.right * distance);

        points.Clear();
        points.Add(ray.origin);
        Vector2 direction = transform.up;
        Vector2 newDirection = Vector2.zero;
        int i = 0;
        int layer;
        do
        {
            hits = Physics2D.RaycastAll(points[points.Count - 1], direction, distance, layers);
            foreach (RaycastHit2D hit in hits)
            {

                if (hit && hit.collider != null)
                {
                    if (hit.collider.isTrigger)
                    {
                        /*if (hit.collider.tag == "LaserLightReactor")
                        {
                            LaserLightReactor llr = hit.collider.GetComponent<LaserLightReactor>();
                            llr.addRay();
                        }*/
                    }
                    else
                    {
                        layer = hit.transform.gameObject.layer;
                        if (layer != LayerMask.NameToLayer("Reflectors") &&
                            layer != LayerMask.NameToLayer("Transparents")&& 
                            layer != LayerMask.NameToLayer("TransparentPhysGround"))
                        {
                            points.Add(hit.point);
                            //Debug.DrawRay(points[i], direction, Color.blue);
                            i = bounces;
                            break;
                        }
                        else // if (layer == LayerMask.NameToLayer("Reflectors"))
                        {
                            points.Add(hit.point + hit.normal * 0.001f);
                            newDirection = Vector2.Reflect((points[points.Count - 1] - (points[points.Count - 2])).normalized, hit.normal);
                            Debug.DrawRay(points[i], direction, Color.cyan);
                            if (newDirection != direction)
                            {
                                direction = newDirection;
                                break;
                            }
                            else
                            {
                                i = bounces;
                                break;
                            }
                        }
                    }
                }
            }
        }
        while (i++ < bounces);
        /*
        for (int j = lights.Count; j < points.Count; j++)
        {
            Transform l = Instantiate(lightPrefab);
            l.transform.position = new Vector3(points[j].x, points[j].y, l.transform.position.z);
            lights.Add(l);
        }*/

        /*
        // проверка освещения
        int rays = points.Count;
        for(int j = 0; j < reflectionsForAreas.Length; j++)
        {
            if (rays >= reflectionsForAreas[j]) // костыли
            {
                rays -= reflectionsForAreas[j];
                areas[j].GetComponent<Light>().intensity = 4.5f;
                if (rays == 0)
                    break;
            }
            else if (rays > 0 && rays < reflectionsForAreas[j])
            {
                areas[j].GetComponent<Light>().intensity = 4.5f * (rays / (float)reflectionsForAreas[j]);
                break;
            }
            else if (rays <= 0)
                break;
        }*/


        line.numPositions = points.Count;
        line.SetPositions(points.ToArray());
    }
    private void FixedUpdate()
    {
        int rays = points.Count;
        for (int j = 0; j < reflectionsForAreas.Length; j++)
        {
            if (rays >= reflectionsForAreas[j]) // костыли
            {
                rays -= reflectionsForAreas[j];
                lightAreas[j].color = Color.clear;
                colliderAreas[j].enabled = false;
            }
            else if (rays > 0 && rays < reflectionsForAreas[j])
            {
                lightAreas[j].color = new Color(0, 0, 0, (0.85f - rays / (float)reflectionsForAreas[j]) * 0.85f);
                colliderAreas[j].enabled = false;
                rays = 0;
            }
            else if (rays <= 0)
            {
                lightAreas[j].color = Color.black;
                colliderAreas[j].enabled = true;
            }
                
        }
    }
}

