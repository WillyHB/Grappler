using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public abstract class Rope : MonoBehaviour
{
    protected LineRenderer lineRenderer;

    protected readonly List<RopeSegment> ropeSegments = new List<RopeSegment>();


    public int NumberOfSegments = 35;

    protected int numberOfSegments;

    public float LengthBetweenSegments = 0.25f;

    public float LineWidth = 0.1f;

    public int ConstraintAppliedPerFrame = 100;

    public float GravityScale = 1;

    [Min(0f)]
    public float GustPowerFrom;
    [Min(0f)]
    public float GustPowerTo;
    
    
    public Vector2 NormalizedWindDirection;

    protected void Simulate()
    {
        // SIMULATION
        for (int i = 1; i < NumberOfSegments; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;

            if (i != ropeSegments.Count - 1 && i != 0)
            {
                velocity += Random.Range(GustPowerFrom, GustPowerTo) * Time.deltaTime * NormalizedWindDirection.normalized;
            }

            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow.y -= GravityScale * Time.fixedDeltaTime;

            ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < ConstraintAppliedPerFrame; i++)
        {
            ApplyConstraint();

        }
    }

    protected abstract void ApplyConstraint();


    protected void ModifyLength(Vector2? pos = null)
    {
        numberOfSegments = NumberOfSegments;

        if (NumberOfSegments > 0 && LengthBetweenSegments > 0)
        {
            while (NumberOfSegments > ropeSegments.Count)
            {
                ropeSegments.Add(new RopeSegment(pos ?? new Vector2(ropeSegments[^1].posNow.x, ropeSegments[^1].posNow.y - LengthBetweenSegments)));
            }

            while (NumberOfSegments < ropeSegments.Count)
            {
                ropeSegments.RemoveAt(ropeSegments.Count - 1);
            }
        }
    }
    protected void ResetRope(Vector3 startPosition)
    {
        ropeSegments.Clear();

        if (NumberOfSegments > 0 && LengthBetweenSegments > 0)
        {
            Vector3 ropeStartPoint = startPosition;

            for (int i = 0; i < NumberOfSegments; i++)
            {
                ropeSegments.Add(new RopeSegment(ropeStartPoint));
                ropeStartPoint.y -= LengthBetweenSegments;
            }

        }
    }

    /*
    public float GetLength()
    {
        Vector3[] points = new Vector3[lineRenderer.positionCount];

        lineRenderer.GetPositions(points);

        float length = 0;

        for (int i = 1; i < points.Length; i++)
        {
            length += (points[i] - points[i - 1]).magnitude;
        }

        return length;
    }*/

    public float GetLength()
    {
        return NumberOfSegments * LengthBetweenSegments;
    }

    public void SetLength(float Length)
    {
        LengthBetweenSegments = Length / NumberOfSegments;
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
}

