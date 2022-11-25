using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPURope : MonoBehaviour
{
    private MeshFilter meshFilter;
    public MeshRenderer meshRenderer { get; set; }
    private BoxCollider2D boxCollider;

    [Header("Rope Settings")]

    public int NumberOfSegments = 35;

    protected int numberOfSegments;

    public float LengthBetweenSegments = 0.25f;

    public float LineWidth = 0.1f;

    public int ConstraintAppliedPerFrame = 100;

    public float GravityScale = 1;
    public float Friction = 1;


    [Space(10)]
    [Header("Wind")]

    [Min(0f)]
    public float GustPowerFrom;
    [Min(0f)]
    public float GustPowerTo;

    public Vector2 NormalizedWindDirection;


    public void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        InitMesh();
    }

    private void InitMesh()
    {      
        Vector3[] vertices = new Vector3[NumberOfSegments * 4];

        int[] triangles = new int[NumberOfSegments * 6];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0; i < vertices.Length; i += 2)
        {
            //vertices[i+1] = new Vector3(-LineWidth / 2, ((float)i / vertices.Length) - 0.5f, 0);
            //vertices[i] = new Vector3(LineWidth / 2, ((float)i / vertices.Length) - 0.5f, 0);

            vertices[i + 1] = Vector3.zero;
            vertices[i] = Vector3.zero;

            uv[i] = vertices[i];
            uv[i + 1] = vertices[i + 1];
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

    public void Update()
    {
        if (NumberOfSegments != numberOfSegments)
        {
            InitMesh();
        }
    }
}
