using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Water : MonoBehaviour
{
    private List<Spring> springs = new();

    public float Tension = 0.025f;
    public float Dampening = 0.025f;
    public float Spread = 0.025f;
    public int NumberOfPoints => (int)(transform.localScale.x / PointDensity);
    private int numberOfPoints;
    public float PointDensity = 0.1f;

    public float SplashMultiplier = 0.075f;

    [Range(0, 1)]
    public float ColliderSurfaceHeight = 1;

    private MeshFilter meshFilter;
    public MeshRenderer meshRenderer { get; set; }
    private BoxCollider2D boxCollider;
    public Bounds Bounds { get; private set; }

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
        boxCollider = GetComponent<BoxCollider2D>();

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
        springs = new List<Spring>
        {
            Capacity = NumberOfPoints
        };

        for (int i = 0; i < NumberOfPoints; i++)
        {
            springs.Add(new Spring());
        }

        InitMesh();
    }

    public void Update()
    {
        float size = (ColliderSurfaceHeight + meshRenderer.bounds.size.y) / meshRenderer.bounds.size.y;

        boxCollider.size = new Vector2(1, size);
        boxCollider.offset = new Vector2(0, (size - 1) / 2f);

        if (numberOfPoints != NumberOfPoints)
        {
            InitSprings();
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

        for (int i = 0; i < NumberOfPoints; i++)
        {
            points[i] = springs[i].Height;
        }

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
                velocity * SplashMultiplier);

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStateMachine>().CurrentWater = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStateMachine>().CurrentWater = null;
        }
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

