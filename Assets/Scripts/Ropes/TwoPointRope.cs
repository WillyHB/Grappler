using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPointRope : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;

    private LineRenderer lineRenderer;
    private readonly List<RopeSegment> ropeSegments = new List<RopeSegment>();

    private float lengthBetweenSegments;
    private int numberOfSegments;

    public int SegmentFrequency = 3;
    private int segmentFrequency;
    public float LineLength = 10;
    private float lineLength;

    public float LineWidth = 0.1f;

    public int ConstraintAppliedPerFrame = 100;

    public float GravityScale = 1;

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        ResetRope();
    }
   

    private void ResetRope()
    {
        lineLength = LineLength;
        segmentFrequency = SegmentFrequency;

        ropeSegments.Clear();

        numberOfSegments = (int)(SegmentFrequency * LineLength);
        lengthBetweenSegments = (float)LineLength / numberOfSegments;

        Vector3 ropeStartPoint = StartPoint.position;

        for (int i = 0; i < numberOfSegments; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= lengthBetweenSegments;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SegmentFrequency != segmentFrequency || LineLength != lineLength)
        {
            ResetRope();
        }

        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();
    }

    private void Simulate()
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

    private void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = StartPoint.position;
        ropeSegments[0] = firstSegment;

        //Constrant to Second Point 
        RopeSegment endSegment = ropeSegments[ropeSegments.Count - 1];
        endSegment.posNow = EndPoint.position;
        ropeSegments[^1] = endSegment;

        for (int i = 0; i < numberOfSegments - 1; i++)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = dist - lengthBetweenSegments;

            Vector2 changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;

            Vector2 changeAmount = changeDir * error;

            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope()
    {
        lineRenderer.startWidth = LineWidth;
        lineRenderer.endWidth = LineWidth;

        Vector3[] ropePositions = new Vector3[numberOfSegments];
        for (int i = 0; i < numberOfSegments; i++)
        {
            ropePositions[i] = ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
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
