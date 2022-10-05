using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Water : MonoBehaviour
{
    private List<Spring> springs = new List<Spring>();

    public float Tension = 0.025f;
    public float Dampening = 0.025f;
    public float Spread = 0.025f;
    public int NumberOfPoints => (int)(transform.localScale.x / PointDensity);
    private int numberOfPoints;
    public float PointDensity = 0.1f;

    public float SplashDownMultiplier = 0.075f;
    public float SplashUpMultiplier = 0.5f;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private EdgeCollider2D edgeCollider2D;

    public Vector2 Splashz;

    public void Splash(int index, float speed)
    {
        if (index >= 0 && index < NumberOfPoints)
        {
            springs[index].Velocity = speed;
        }
    }


    public void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        edgeCollider2D = GetComponent<EdgeCollider2D>();

        InitSprings();
    }

    private void InitMesh()
    {      
        Vector3[] vertices = new Vector3[NumberOfPoints * 2];

        int numberOfVertices = (NumberOfPoints * 2);
        int boxNumber = 0;

        if ((numberOfVertices -= 4) > 0)
        {
            boxNumber++;
        }

        while (numberOfVertices > 0)
        {
            numberOfVertices -= 2;
            boxNumber++;
        }

        int[] triangles = new int[boxNumber*6];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0; i < vertices.Length; i += 2)
        {
            vertices[i] = new Vector3(-0.5f + ((float)i / vertices.Length), -0.5f, 0);
            vertices[i + 1] = new Vector3(-0.5f + ((float)i / vertices.Length), 0.5f, 0);

            uv[i] = new Vector3(-0.5f + ((float)i / vertices.Length), -0.5f, 0);
            uv[i + 1] = new Vector3(-0.5f + ((float)i / vertices.Length), 0.5f, 0);
        }


        for (int i = 0; i < triangles.Length; i+= 6)
        {
            int j = i / 3;
            triangles[i + 0] = j;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
            triangles[i + 3] = j + 1;
            triangles[i + 4] = j + 3;
            triangles[i + 5] = j + 2;
        }

        

        Mesh mesh = new()
        {
            name = "WaterMesh"
        };

        meshFilter.mesh.Clear();
        meshFilter.mesh = mesh;



        meshFilter.mesh.SetVertices(vertices);
        meshFilter.mesh.SetTriangles(triangles, 0);
        meshFilter.mesh.SetUVs(0, uv);

    }

    private void InitSprings()
    {
        numberOfPoints = NumberOfPoints;


        springs.Clear();
        springs = new List<Spring>();
        springs.Capacity = NumberOfPoints;

        for (int i = 0; i < NumberOfPoints; i++)
        {
            springs.Add(new Spring());
        }

        InitMesh();
    }

    public void Update()
    {
        if (numberOfPoints != NumberOfPoints)
        {
            InitSprings();
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Splash((int)Splashz.x, Splashz.y);
        }
        for (int i = 0; i < NumberOfPoints; i++)
        {
            springs[i].Update(Dampening, Tension);
        }

        float[] leftDeltas = new float[NumberOfPoints];
        float[] rightDeltas = new float[NumberOfPoints];

        // do some passes where springs pull on their neighbours
        for (int i = 0; i < NumberOfPoints; i++)
        {
            if (i > 0)
            {
                leftDeltas[i] = Spread * (springs[i].Height - springs[i - 1].Height);
                springs[i - 1].Velocity += leftDeltas[i];
            }
            if (i < NumberOfPoints - 1)
            {
                rightDeltas[i] = Spread * (springs[i].Height - springs[i + 1].Height);
                springs[i + 1].Velocity += rightDeltas[i];
            }
        }

        for (int i = 0; i < NumberOfPoints; i++)
        {
            if (i > 0)
                springs[i - 1].Height += leftDeltas[i];
            if (i < NumberOfPoints - 1)
                springs[i + 1].Height += rightDeltas[i];


        }

        float[] points = new float[2000];
        List<Vector2> pointz = new();

        for (int i = 0; i < NumberOfPoints; i++)
        {
            points[i] = springs[i].Height;

            pointz.Add(
                new Vector2(
                    (float)i / NumberOfPoints, 
                    (springs[i].Height + meshRenderer.bounds.size.y) / meshRenderer.bounds.size.y));
        }

        edgeCollider2D.SetPoints(pointz);
        edgeCollider2D.offset = new Vector2(-0.5f, -0.5f);

        meshRenderer.material.SetFloatArray("points", points);
        meshRenderer.material.SetVector("size", 
            new Vector4(meshRenderer.bounds.size.x, meshRenderer.bounds.size.y, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float velocity = collision.attachedRigidbody.velocity.y;

        float x = collision.transform.position.x;

        Splash(
                (int)((x - (transform.position.x - meshRenderer.bounds.size.x / 2)) / meshRenderer.bounds.size.x * NumberOfPoints),
                velocity * (velocity > 0 ? SplashUpMultiplier : SplashDownMultiplier));
    }

    public class Spring
    {
        public float Height { get; set; } = 1;
        public float Velocity { get; set; }

        public void Update(float dampening, float tension)
        {

            float x = Height - 1;

            Velocity += -tension * x - dampening * Velocity;

            Height += Velocity;
        }

    }
}

