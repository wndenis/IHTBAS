using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour
{

    public static float getGameObjectWidth(GameObject obj)
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        SpriteRenderer[] sprites = obj.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in sprites)
        {


            float tmpMinX = sr.transform.position.x + sr.sprite.bounds.center.x - sr.sprite.bounds.size.x * sr.transform.lossyScale.x / 2f;// - sr.sprite.bounds.size.x * sr.transform.lossyScale.x/2f;
            float tmpMaxX = sr.transform.position.x + sr.sprite.bounds.center.x + sr.sprite.bounds.size.x * sr.transform.lossyScale.x / 2f;// + sr.sprite.bounds.size.x* sr.transform.lossyScale.x/2f;



            if (tmpMinX < minX)
            {
                minX = tmpMinX;
            }
            if (tmpMaxX > maxX)
            {
                maxX = tmpMaxX;
            }
        }
        return (maxX - minX);
    }

    public static float getGameObjectMinX(GameObject obj)
    {
        float minX = float.MaxValue;

        SpriteRenderer[] sprites = obj.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in sprites)
        {
            float tmpMinX = sr.transform.position.x + sr.sprite.bounds.center.x - sr.sprite.bounds.size.x * sr.transform.lossyScale.x / 2f;

            if (tmpMinX < minX)
            {
                minX = tmpMinX;
            }
        }
        return minX;
    }

    public static float getGameObjectMaxX(GameObject obj)
    {

        return getGameObjectMinX(obj) + getGameObjectWidth(obj);
    }


    public static float getGameObjectMinY(GameObject obj)
    {
        float minY = float.MaxValue;

        SpriteRenderer[] sprites = obj.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in sprites)
        {
            float tmpMinY = sr.transform.position.y + sr.sprite.bounds.center.y - sr.sprite.bounds.size.y * sr.transform.lossyScale.y / 2f;

            if (tmpMinY < minY)
            {
                minY = tmpMinY;
            }
        }
        return minY;
    }

    public static float getGameObjectMaxY(GameObject obj)
    {

        return getGameObjectMinY(obj) + getGameObjectHeight(obj);
    }


    public static float getGameObjectHeight(GameObject obj)
    {
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        SpriteRenderer[] sprites = obj.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in sprites)
        {
            float tmpMinY = sr.transform.position.y + sr.sprite.bounds.center.y - sr.sprite.bounds.size.y * sr.transform.lossyScale.y / 2f;
            float tmpMaxY = sr.transform.position.y + sr.sprite.bounds.center.y + sr.sprite.bounds.size.y * sr.transform.lossyScale.y / 2f;

            if (tmpMinY < minY)
            {
                minY = tmpMinY;
            }
            if (tmpMaxY > maxY)
            {
                maxY = tmpMaxY;
            }
        }
        return (maxY - minY);
    }



    public static void setSortingLayer(GameObject t, string layerName = "Default")
    {
        SpriteRenderer[] sprites = t.GetComponentsInChildren<SpriteRenderer>();
        if (sprites.Length != 0)
        {
            foreach (SpriteRenderer s in sprites)
            {
                s.sortingLayerName = layerName;
            }
        }
    }

    public static void SetActiveRecursively(GameObject rootObject, bool active)
    {
        //rootObject.SetActive(active);

        foreach (Transform childTransform in rootObject.transform)
        {
            SetActiveRecursively(childTransform.gameObject, active);
        }
    }

    public static void SetActiveChild(Transform rootObject, bool active)
    {
        //rootObject.SetActive(active);

        foreach (Transform childTransform in rootObject)
        {
            childTransform.gameObject.SetActive(active);
        }
    }
}