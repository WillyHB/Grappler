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

    [Space(10)]
    [Header("Wind")]

    public float GustPowerFrom;
    [Min(0f)]
    public float GustPowerTo;

    public Vector2 NormalizedWindDirection;

    private List<GameObject> spheres;
    private List<Particle> particles;
    private List<Connector> connectors;

    public class Connector
    {
        public bool enabled = true;
        public LineRenderer lineRender;
        public GameObject p0;
        public GameObject p1;
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
        meshFilter = GetComponent<MeshFilter>();

        Vector2 spawnParticlePos = new Vector2(0, 0);

        spheres = new List<GameObject>();
        particles = new List<Particle>();
        connectors = new List<Connector>();

        for (int y = 0; y <= Rows; y++)
        {
            for (int x = 0; x <= Columns; x++)
            {
                // Create a sphere
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.layer = 11;
                sphere.transform.parent = transform;

                var mat = sphere.GetComponent<Renderer>();
                sphere.transform.localPosition = new Vector2(spawnParticlePos.y, spawnParticlePos.x);
                sphere.transform.localScale = new Vector2(0.2f, 0.2f);

                // Create particle
                Particle point = new Particle();
                point.pinnedPos = new Vector2(spawnParticlePos.y, spawnParticlePos.x);

                // Create a vertical connector 
                if (x != 0)
                {
                    LineRenderer line = new GameObject("Line").AddComponent<LineRenderer>();
                    line.gameObject.layer = 11;
                    line.useWorldSpace = false;

                    Connector connector = new Connector();
                    connector.p0 = sphere;
                    connector.p1 = spheres[spheres.Count - 1]; //

                    connector.point0 = point;
                    connector.point1 = particles[particles.Count - 1]; //
                    connector.point0.pos = sphere.transform.localPosition;
                    connector.point0.oldPos = sphere.transform.localPosition;

                    connectors.Add(connector);

                    connector.lineRender = line;
                    connector.lineRender.transform.parent = transform;
                    connector.lineRender.transform.localPosition = Vector2.zero;
                }

                // Create a horizontal connector
                if (y != 0)
                {
                    LineRenderer line = new GameObject("Line").AddComponent<LineRenderer>();
                    line.gameObject.layer = 11;
                    line.useWorldSpace = false;

                    Connector connector = new Connector();
                    connector.p0 = sphere;
                    connector.p1 = spheres[(y - 1) * (Rows + 1) + x]; //

                    connector.point0 = point;
                    connector.point1 = particles[(y - 1) * (Rows + 1) + x]; //
                    connector.point0.pos = sphere.transform.localPosition;
                    connector.point0.oldPos = sphere.transform.localPosition;

                    connectors.Add(connector);

                    connector.lineRender = line;
                    connector.lineRender.transform.parent = transform;
                    connector.lineRender.transform.localPosition = Vector2.zero;
                }

                // Pin the points in the top row of the grid
                if (x == 0)
                {
                    point.pinned = true;
                }

                spawnParticlePos.x -= Spacing;

                // Add particle and spehere to lists
                spheres.Add(sphere);
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

        int xCapacity = (Columns - 1);
        int yCapacity = (Rows - 1);

        for (float i = 0; i < Rows; i++)
        {
            for (float j = 0; j < Columns; j++)
            {
                vertices.Add(new Vector3(j / Columns, i / Rows, 0));
            }
        }
        for (int i = 0; i < yCapacity; i++)
        {
            for (int j = 0; j < xCapacity; j++)
            {
                triangles.Add(j + (i + 1) * Columns);
                triangles.Add((j + 1) + i * Columns);
                triangles.Add(j + i * Columns);

                triangles.Add((j + 1) + (i + 1) * Columns);
                triangles.Add((j + 1) + i * Columns);
                triangles.Add(j + (i + 1) * Columns);
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

    private void FixedUpdate()
    {
        Simulate();
        Constrain();
        Draw();
    }

    private void Draw()
    {
        for (int p = 0; p < particles.Count; p++)
        {
            Particle point = particles[p];
            spheres[p].transform.localPosition = new Vector2(point.pos.x, point.pos.y);
            spheres[p].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        }

        // Set lines
        for (int i = 0; i < connectors.Count; i++)
        {
            if (connectors[i].enabled == false)
            {
                Destroy(connectors[i].lineRender);
            }

            else
            {
                // Set points for the lines
                var points = new Vector3[2];
                points[0] = connectors[i].p0.transform.localPosition + new Vector3(0, 0, 1);
                points[1] = connectors[i].p1.transform.localPosition + new Vector3(0, 0, 1);

                // Draw lines
                connectors[i].lineRender.startWidth = 0.04f;
                connectors[i].lineRender.endWidth = 0.04f;
                connectors[i].lineRender.SetPositions(points);

            }

        }
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
            if (connectors[i].enabled == false)
            {
                // Do nothing
            }

            else
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
}