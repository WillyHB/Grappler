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
    public float WaveSpeed = 1;
    public float PointDensity = 0.1f;
    public float WaveAmplitude;

    public int NumberOfPoints => (int)(PointDensity * GetComponent<SpriteRenderer>().bounds.size.x);

    [Range(0, 1)]
    public float TargetHeight;

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
        springs.Clear();
        springs = new List<Spring>();
        springs.Capacity = NumberOfPoints;

        for (int i = 0; i < NumberOfPoints; i++)
        {
            springs.Add(new Spring(GetComponent<SpriteRenderer>().size.y - GetComponent<SpriteRenderer>().bounds.size.y * TargetHeight));
        }
    }

    public void Update()
    {
        Debug.Log(springs.Count);
        if (springs.Count != NumberOfPoints)
        {
            Start();
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Splash((int)Splashz.x, Splashz.y);
        }
        for (int i = 0; i < NumberOfPoints; i++)
        {
            /*
            springs[i].Height += 0.5f * WaveAmplitude * Mathf.Sin((i * 0.1f + Time.time*WaveSpeed));
            springs[i].Height += 0.3f * WaveAmplitude * Mathf.Sin((i * 0.15f - Time.time*WaveSpeed));
            */
            springs[i].Update(Dampening, Tension, GetComponent<SpriteRenderer>().size.y - GetComponent<SpriteRenderer>().bounds.size.y * TargetHeight);
        }

        float[] leftDeltas = new float[NumberOfPoints];
        float[] rightDeltas = new float[NumberOfPoints];

        // do some passes where springs pull on their neighbours
        for (int j = 0; j < 8; j++)
        {
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
        }

        Vector4[] points = new Vector4[2000];


        for (int i = 0; i < NumberOfPoints; i++)
        {
            points[i] = new Vector4((GetComponent<SpriteRenderer>().bounds.size.x / NumberOfPoints) * i, springs[i].Height, 0, 0);

        }

        Debug.Log(points[50]);

        GetComponent<SpriteRenderer>().material.SetVectorArray("springPoints", points.ToList());
        GetComponent<SpriteRenderer>().material.SetFloat("springCount", NumberOfPoints);
        GetComponent<SpriteRenderer>().material.SetVector("size", GetComponent<SpriteRenderer>().size);
    }

    public class Spring
    {
        public Spring(float height)
        {
            Height = height;
        }
        public float Height;
        public float Velocity { get; set; }

        public void Update(float dampening, float tension, float targetHeight)
        {

            float x = Height - targetHeight;

            Velocity += -tension * x - dampening * Velocity;

            Height += Velocity;
        }

    }
}

