using System.Collections.Generic;
using UnityEngine;

public class VelvetCloth : MonoBehaviour
{

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public int Rows = 16;
    public int Columns = 16;
    public float Spacing = 0.25f;
    public float Gravity = 0.24f;
    public float Friction = 0.99f;
    public int ConstraintsPerFrame = 1;
    private int rows;
    private int columns;
    private float spacing;

    private Vector2 worldPos;

    [Space(10)]
    [Header("Wind")]

    public float GustPowerFrom;
    [Min(0f)]
    public float GustPowerTo;

    public Vector2 NormalizedWindDirection;

    [Header("Collision")]
    public bool CollideWithRigidbodies;
    public float Stiffness;

    private List<Particle> particles;
    private List<Connector> connectors;

    public class Connector
    {
        public Particle point0;
        public Particle point1;
        public Vector2 changeDir;
    }

    public class Particle
    {
        public Vector2 baseOffset;
        public bool pinned = false;
        public Vector2 pos;
        public Vector2 oldPos;
        public Vector2 vel;
    }

    // Initalize
    void Start()
    {
        rows = Rows;
        columns = Columns;
        spacing = Spacing;

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        Vector2 spawnParticlePos = transform.position;

        particles = new List<Particle>();
        connectors = new List<Connector>();

        for (int y = 0; y <= Rows; y++)
        {
            for (int x = 0; x <= Columns; x++)
            {
                Vector2 spawnPos = new(spawnParticlePos.x, spawnParticlePos.y);
                // Create particle
                Particle point = new()
                {
                    baseOffset = spawnPos - (Vector2)transform.position
                };

                // Create a vertical connector 
                if (y != 0)
                {
                    Connector connector = new()
                    {
                        point0 = point,
                        point1 = particles[x + (y - 1) * (Columns + 1)] //
                    };

                    connector.point0.pos = spawnPos;
                    connector.point0.oldPos = spawnPos;

                    connectors.Add(connector);
                }

                // Create a horizontal connector
                if (x != 0)
                {
                    Connector connector = new()
                    {
                        point0 = point,
                        point1 = particles[^1] //
                    };
                    connector.point0.pos = spawnPos;
                    connector.point0.oldPos = spawnPos;

                    connectors.Add(connector);
                }

                // Pin the points in the top row of the grid
                if (y == 0)
                {
                    point.pinned = true;
                }

                spawnParticlePos.x += Spacing;

                particles.Add(point);
            }

            spawnParticlePos.x = 0;
            spawnParticlePos.y += Spacing;
        }

        InitMesh();
    }

    private void InitMesh()
    {
        List<Vector3> vertices = new();
        List<Vector2> uv = new();
        List<int> triangles = new();

        int Width = Columns + 1;
        int Height = Rows + 1;

        
        for (float i = Height-1; i >= 0; i--)
        {
            for (float j = 0; j < Width; j++)
            {
                vertices.Add(new Vector3(j / Width, i / Height, 0));

            }
        }
        for (int i = 0; i < vertices.Count; i++)
        {
            uv.Add(vertices[i]);
        }

        for (int i = Rows-1; i >= 0; i--)
        {
            for (int j = 0; j < Columns; j++)
            {
                triangles.Add(j + i * Width);
                triangles.Add((j + 1) + i * Width);
                triangles.Add(j + (i + 1) * Width);

                triangles.Add(j + (i + 1) * Width);
                triangles.Add((j + 1) + i * Width);
                triangles.Add((j + 1) + (i + 1) * Width);
            }
        }


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

    private void Update()
    {
        if ((Vector2)transform.position != worldPos)
        {
            foreach (Particle p in particles)
            {
                if (p.pinned)
                {
                    p.pos = transform.position;
                    p.oldPos = transform.position;
                }
            }
        }
        if (rows != Rows
            || columns != Columns
            || spacing != Spacing)
        {
            Start();
        }
    }

    private void FixedUpdate()
    {
        Simulate();
        
        for (int i = 0; i < ConstraintsPerFrame; i++)
        {
            Constrain();
        }

        if (CollideWithRigidbodies)
        {
            GenerateCollider();
        }
        Draw();
    }



    private void Draw()
    {
        List<Vector3> vertices = new();
        Vector4[] points = new Vector4[2000];
        meshFilter.mesh.GetVertices(vertices);

        for (int p = 0; p < particles.Count; p++)
        {
            Particle point = particles[p];

            points[p] = new Vector4(point.pos.x - transform.position.x , point.pos.y - transform.position.y);
            vertices[p] = point.pos;
        }

        meshRenderer.material.SetVectorArray("points", points);
        meshRenderer.material.SetVector("size", new Vector4((Columns +1) * Spacing, (Rows + 1) * Spacing, 0, 0));
    }

    private void Simulate()
    {
        for (int p = 0; p < particles.Count; p++)
        {
            Particle point = particles[p];
            if (point.pinned == true)
            {
                point.pos = new Vector2(transform.position.x + point.baseOffset.x, transform.position.y);
                point.oldPos = new Vector2(transform.position.x + point.baseOffset.x, transform.position.y);
            }

            else
            {
                point.vel = (point.pos - point.oldPos) * Friction;
                point.vel += (Random.Range(GustPowerFrom, GustPowerTo) * Time.deltaTime * NormalizedWindDirection.normalized);

                point.oldPos = point.pos;

                point.pos += point.vel;
                point.pos.y -= Gravity * Time.fixedDeltaTime;
            }
        }
    }

    private void Constrain()
    {
        for (int i = 0; i < connectors.Count; i++)
        {
            float dist = (connectors[i].point0.pos - connectors[i].point1.pos).magnitude;
            float error = Mathf.Abs(dist - Spacing);

            if (dist > Spacing)
            {
                connectors[i].changeDir = (connectors[i].point0.pos - connectors[i].point1.pos).normalized;
            }
            else if (dist < Spacing)
            {
                connectors[i].changeDir = (connectors[i].point1.pos - connectors[i].point0.pos).normalized;
            }

            Vector2 changeAmount = connectors[i].changeDir * error;
            if (!connectors[i].point0.pinned) connectors[i].point0.pos -= changeAmount * 0.5f;
            if (!connectors[i].point1.pinned) connectors[i].point1.pos += changeAmount * 0.5f;           
        }
    }

    private void GenerateCollider()
    {
        GetComponent<MeshCollider>().sharedMesh = meshFilter.sharedMesh;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CollideWithRigidbodies)
        {
            foreach (Particle particle in particles)
            {
                if (collision.bounds.Contains(particle.pos))
                {
                    particle.pos += collision.GetComponent<Rigidbody2D>().velocity / Stiffness;
                }
            }
        }
    }
}