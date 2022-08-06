using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider2D : MonoBehaviour
{
    public LineRenderer LineRenderer;
    private EdgeCollider2D edgeCollider;

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
       

        edgeCollider.points = GetPoints();
    }

    private Vector2[] GetPoints()
    {
        Vector2[] points = new Vector2[LineRenderer.positionCount];

        for (int i = 0; i < LineRenderer.positionCount; i++)
        {
            points[i] = LineRenderer.GetPosition(i);
        }

        return points;
    }
}
