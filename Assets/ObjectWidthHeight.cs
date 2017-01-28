using UnityEngine;
using System.Collections;

public class ObjectWidthHeight : MonoBehaviour
{
    public GameObject parent;
    public float Width;
    public float Height;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    void Start()
    {
        parent = this.gameObject;
        Width = Utils.getGameObjectWidth(parent);
        Height = Utils.getGameObjectHeight(parent);

    }

    void Update()
    {
        if (parent != null)
        {
            minY = Utils.getGameObjectMinY(this.gameObject);
            Vector3 startY = new Vector3(transform.position.x, minY, 0);
            Vector3 endY = new Vector3(transform.position.x, minY + Utils.getGameObjectHeight(this.gameObject), 0);
            Debug.DrawLine(startY, endY);

            minX = Utils.getGameObjectMinX(this.gameObject);
            Vector3 startX = new Vector3(minX, transform.position.y, 0);
            Vector3 endX = new Vector3(minX + Utils.getGameObjectWidth(this.gameObject), transform.position.y, 0);
            Debug.DrawLine(startX, endX);
            Width = Utils.getGameObjectWidth(parent);
            Height = Utils.getGameObjectHeight(parent);
            maxX = Utils.getGameObjectMaxX(this.gameObject);
            maxY = Utils.getGameObjectMaxY(this.gameObject);
        }
    }
}