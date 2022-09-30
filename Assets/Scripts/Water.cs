using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Water : MonoBehaviour
{
    public float Tension = 0.025f;
    public float Dampening = 0.025f;
    public float Spread = 0.025f;
    public float WaveSpeed = 1;
    public int NumberOfPoints = 100;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    public float TargetHeight;

    public ComputeShader ComputeShader;
    private ComputeBuffer springsBuffer;
    private ComputeBuffer pointBuffer;

    public Vector2 Splashz;

    public void Splash(int index, float speed)
    {
        
    }

    public void OnDisable()
    {
        pointBuffer.Dispose();
        springsBuffer.Dispose();
    }

    public void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        springsBuffer = new ComputeBuffer(2000, 8);
        pointBuffer = new ComputeBuffer(NumberOfPoints, 4);

        ComputeShader.SetBuffer(0, "springs", springsBuffer);
        ComputeShader.SetBuffer(0, "points", pointBuffer);

        InitSprings();
    }

    private void InitMesh()
    {

        Vector3[] vertices = new Vector3[NumberOfPoints * 2];
        int[] triangles = new int[(int)(((NumberOfPoints * 2.0f) / 4.0f) * 6.0f)];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0; i < vertices.Length; i += 2)
        {

            vertices[i] = new Vector3(-0.5f + (i / (NumberOfPoints * 2.0f)), -0.5f, 0);
            vertices[i + 1] = new Vector3(-0.5f + (i / (NumberOfPoints * 2.0f)), 0.5f, 0);

            uv[i] = new Vector3(-0.5f + (i / (NumberOfPoints * 2.0f)), -0.5f, 0);
            uv[i + 1] = new Vector3(-0.5f + (i / (NumberOfPoints * 2.0f)), 0.5f, 0);
        }

        int j = 0;
        for (int i = 0; i < triangles.Length; i += 6)
        {
            triangles[i + 0] = j;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
            triangles[i + 3] = j + 2;
            triangles[i + 4] = j + 1;
            triangles[i + 5] = j + 3;

            j += 2;
        }



        Mesh mesh = new()
        {
            name = "WaterMesh"
        };

        meshFilter.mesh.Clear();
        meshFilter.mesh = mesh;



        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.triangles = triangles;
        meshFilter.mesh.uv = uv;
    }

    private void InitSprings()
    {
        InitMesh();
    }

    public void Update()
    {
        ComputeShader.SetFloat("tension", Tension);
        ComputeShader.SetFloat("dampening", Dampening);
        ComputeShader.SetFloat("spread", Spread);
        ComputeShader.SetFloat("waveSpeed", WaveSpeed);
        ComputeShader.SetInt("numberOfPoints", NumberOfPoints);
        ComputeShader.Dispatch(0, 8, 1, 1);

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Splash((int)Splashz.x, Splashz.y);
        }

        meshRenderer.material.SetFloat("springCount", NumberOfPoints);
        meshRenderer.material.SetVector("size",
            new Vector4(meshRenderer.bounds.size.x, meshRenderer.bounds.size.y, TargetHeight, 0));



    }

    public struct Spring
    {
        public float Height;
        public float Velocity { get; set; }
    }
}

