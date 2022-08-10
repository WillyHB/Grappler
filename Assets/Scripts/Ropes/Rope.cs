using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public abstract class Rope : MonoBehaviour
{
    protected LineRenderer lineRenderer;

    protected readonly List<RopeSegment> ropeSegments = new List<RopeSegment>();

    protected float lengthBetweenSegments;
    protected int numberOfSegments;

    public int SegmentFrequency = 3;
    protected int segmentFrequency;
    public float LineLength = 10;
    protected float lineLength;

    public float LineWidth = 0.1f;

    public int ConstraintAppliedPerFrame = 100;

    public float GravityScale = 1;

    private void Update()
    {
        
    }

    protected void Simulate()
    {
        // SIMULATION
        for (int i = 1; i < numberOfSegments; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
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


    protected void ModifyLength(Vector2 pos)
    {
        numberOfSegments = (int)(SegmentFrequency * LineLength);
        lengthBetweenSegments = (float)LineLength / numberOfSegments;

        while (numberOfSegments > ropeSegments.Count)
        {
            ropeSegments.Add(new RopeSegment(pos));
        }

        while (numberOfSegments < ropeSegments.Count)
        {
            ropeSegments.RemoveAt(ropeSegments.Count - 1);
        }
    }
    protected void ResetRope(Vector3 startPosition)
    {
        lineLength = LineLength;
        segmentFrequency = SegmentFrequency;

        ropeSegments.Clear();

        numberOfSegments = (int)(SegmentFrequency * LineLength);
        lengthBetweenSegments = (float)LineLength / numberOfSegments;

        Vector3 ropeStartPoint = startPosition;

        for (int i = 0; i < numberOfSegments; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= lengthBetweenSegments;
        }
    }

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
