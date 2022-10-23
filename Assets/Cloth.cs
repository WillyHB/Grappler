using System.Collections.Generic;
using UnityEngine;

public class Cloth : MonoBehaviour
{

    private MeshFilter meshFilter;

    public int Rows = 16;
    public int Columns = 16;
    public float Spacing = 0.25f;
    public float Gravity = 0.24f;
    public float Friction = 0.99f;

    private int rows;
    private int columns;
    private float spacing;

    [Space(10)]
    [Header("Wind")]

    public float GustPowerFrom;
    [Min(0f)]
    public float GustPowerTo;

    public Vector2 NormalizedWindDirection;

    private List<Particle> particles;
    private List<Connector> connectors;

    public class Connector
    {
        public bool isHorizontal;
        public Particle point0;
        public Particle point1;
        public Vector2 changeDir;
    }

    public class Particle
    {
        public bool pinned = false;
        public Vector2 pinnedPos;
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

        Vector2 spawnParticlePos = new(0, 0);

        particles = new List<Particle>();
        connectors = new List<Connector>();

        for (int y = 0; y <= Rows; y++)
        {
            for (int x = 0; x <= Columns; x++)
            {
                Vector2 spawnPos = new(spawnParticlePos.y, spawnParticlePos.x);
                // Create particle
                Particle point = new()
                {
                    pinnedPos = spawnPos
                };

                // Create a vertical connector 
                if (x != 0)
                {
                    Connector connector = new()
                    {
                        isHorizontal = false,

                        point0 = point,
                        point1 = particles[^1] //
                    };
                    connector.point0.pos = spawnPos;
                    connector.point0.oldPos = spawnPos;

                    connectors.Add(connector);
                }

                // Create a horizontal connector
                if (y != 0)
                {
                    Connector connector = new()
                    {
                        isHorizontal = true,

                        point0 = point,
                        point1 = particles[x + (y-1) * (Columns + 1)] //
                    };
                    connector.point0.pos = spawnPos;
                    connector.point0.oldPos = spawnPos;

                    connectors.Add(connector);
                }

                // Pin the points in the top row of the grid
                if (x == 0)
                {
                    point.pinned = true;
                }

                spawnParticlePos.x -= Spacing;

                particles.Add(point);
            }

            spawnParticlePos.x = 0;
            spawnParticlePos.y -= Spacing;
        }

        InitMesh();
    }

    private void InitMesh()
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();

        int Width = Columns + 1;
        int Height = Rows + 1;

        int xCapacity = (Width - 1);
        int yCapacity = (Height - 1);

        for (float j = 0; j < Width; j++)
        {
            for (float i = 0; i < Height; i++)
            {
                vertices.Add(new Vector3(j / Width, i / Height, 0));
            }
        }

        for (int j = 0; j < xCapacity; j++)
        {
            for (int i = 0; i < yCapacity; i++)
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
    }

    private void Update()
    {
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
        Constrain();
        Draw();
    }

    private void Draw()
    {
        List<Vector3> vertices = new();

        meshFilter.mesh.GetVertices(vertices);

        for (int p = 0; p < particles.Count; p++)
        {
            Particle point = particles[p];
            vertices[p] = point.pos;
        }

        meshFilter.mesh.SetVertices(vertices);
    }

    private void Simulate()
    {
        for (int p = 0; p < particles.Count; p++)
        {
            Particle point = particles[p];
            if (point.pinned == true)
            {
                point.pos = point.pinnedPos;
                point.oldPos = point.pinnedPos;
            }

            else
            {
                point.vel = (point.pos - point.oldPos) * Friction;
                point.vel += Random.Range(GustPowerFrom, GustPowerTo) * Time.deltaTime * NormalizedWindDirection.normalized;
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
            connectors[i].point0.pos -= changeAmount * 0.5f;
            connectors[i].point1.pos += changeAmount * 0.5f;
        }
    }
}