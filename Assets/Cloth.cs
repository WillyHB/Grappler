using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private Point[] points;
    public float Gravity = 1;

    public int NumberOfPoints { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public int ConstraintIterations = 8;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        Width = 10;
        Height = 10;
        NumberOfPoints = 100;
        points = new Point[NumberOfPoints];

        InitMesh();

        /*
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Point p = new();

                if (j != 0)
                {
                    p.LinkB = points[j - 1];
                }

                if (i != 0)
                {
                    p.LinkB = points[(i - 1) * (Width + 1) + j];
                }

                points[i + j * Width] = p;
            }
        }

        */
    }

    private void InitMesh()
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[6];
        //List<int> triangles = new();
        int[] triangles;

        vertices = new Vector3[]{ new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0)};
        uv = new Vector2[]{ new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0)};
        triangles = new int[] { 0, 1, 2, 3, 0, 2};
        /*
        for (int i = 0; i < vertices.Length; i++)
        {
            int xCoordinate = i % Width;
            int yCoordinate = i / Width;

            vertices[i] = new Vector3((float)xCoordinate / Width, (float)yCoordinate / Height, 0);

            uv[i] = new Vector3((float)xCoordinate / Width, (float)yCoordinate / Height, 0);
        }

        
        for (int i = 0; i < NumberOfPoints; i++)
        {
            int xCoordinate = i % Width;
            int yCoordinate = i / Width;

            if (i % 2 == 0)
            {
                triangles.Add(i);
                triangles.Add(xCoordinate+1 + yCoordinate * Width);
                triangles.Add(xCoordinate + (yCoordinate + 1) * Width);
            }

            else
            {
                triangles.Add(i);
                triangles.Add(xCoordinate-1 + (yCoordinate + 1) * Width);
                triangles.Add(xCoordinate+1 + yCoordinate * Width);
            }

        }
        */

        


        Mesh mesh = new()
        {
            name = "ClothMesh"
        };

        meshFilter.mesh.Clear();
        meshFilter.mesh = mesh;



        meshFilter.mesh.SetVertices(vertices);
        meshFilter.mesh.SetTriangles(triangles, 0);
        meshFilter.mesh.SetUVs(0, uv);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        Simulate();

        for (int i = 0; i < ConstraintIterations; i++)
        {
            Constraint();
        }*/
    }

    public void Simulate()
    {
        for (int i = 0; i < points.Length; i++)
        {
            var p = points[i];

            float vx = p.x - p.oldX;
            float vy = p.y - p.oldY;

            p.oldX = p.x;
            p.oldY = p.y;
            p.x += vx;
            p.y += vy;
            p.y -= Gravity * Time.deltaTime;
        }
    }

    public void Constraint()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Point p1 = points[i];
            Point p2 = points[i].LinkB;
            // calculate the distance
            float distX = p1.x - p2.x;
            float distY = p1.y - p2.y;
            var d = Mathf.Sqrt(distX * distX + distY * distY);

            // difference scalar
            float difference = (points[i].restingDistance - d) / d;

            // translation for each PointMass. They'll be pushed 1/2 the required distance to match their resting distances.
            float translateX = distX * 0.5f * difference;
            float translateY = distY * 0.5f * difference;

            p1.x += translateX;
            p1.y += translateY;

            p2.x -= translateX;
            p2.y -= translateY;

            points[i] = p1;
            points[i].LinkB = p2;
        }
    }


    public class Point
    {
        public float x, y;
        public float oldX, oldY;

        public Point LinkB;
        public float restingDistance = 0.1f;
    }
}
