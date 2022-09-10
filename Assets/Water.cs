using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer), typeof(BoxCollider2D))]
public class Water : MonoBehaviour
{
    private Spring[] springs = new Spring[100];

    private BoxCollider2D boxCollider;
    private LineRenderer lineRenderer;

    public float Tension = 0.025f;
    public float Dampening = 0.025f;
    public float Spread = 0.025f;

    public void Splash(int index, float speed)
    {
        if (index >= 0 && index < springs.Length)
        {
            springs[index].Velocity = speed;
        }
    }

    public void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

        for (int i = 0; i < springs.Length; i++)
        {
            springs[i] = new Spring(boxCollider.size.y);
        }
    }

    public void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            Splash(50, -10);
        }
        for (int i = 0; i < springs.Length; i++)
        {
            springs[i].Update(Dampening, Tension);
        }

        float[] leftDeltas = new float[springs.Length];
        float[] rightDeltas = new float[springs.Length];

        // do some passes where springs pull on their neighbours
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < springs.Length; i++)
            {
                if (i > 0)
                {
                    leftDeltas[i] = Spread * (springs[i].Height - springs[i - 1].Height);
                    springs[i - 1].Velocity += leftDeltas[i];
                }
                if (i < springs.Length - 1)
                {
                    rightDeltas[i] = Spread * (springs[i].Height - springs[i + 1].Height);
                    springs[i + 1].Velocity += rightDeltas[i];
                }
            }

            for (int i = 0; i < springs.Length; i++)
            {
                if (i > 0)
                    springs[i - 1].Height += leftDeltas[i];
                if (i < springs.Length - 1)
                    springs[i + 1].Height += rightDeltas[i];

                
            }
        }

        lineRenderer.positionCount = springs.Length;
        Vector3[] points = new Vector3[springs.Length];

        Debug.Log(boxCollider.size.x);
        for (int i = 0; i < springs.Length; i++)
        {
            points[i] = new Vector3((boxCollider.size.x / springs.Length) * i, springs[i].Height, 0);

        }

       lineRenderer.SetPositions(points);
    }

    public class Spring
    {
        public Spring(float targetHeight)
        {
            Height = targetHeight;
            TargetHeight = targetHeight;
        }
        public float Height;
        public float TargetHeight;
        public float Velocity { get; set; }

        public void Update(float dampening, float tension)
        {
            float x = Height - TargetHeight;

            Velocity += -tension * x - dampening * Velocity;

            Height += Velocity;
        }

    }
}


