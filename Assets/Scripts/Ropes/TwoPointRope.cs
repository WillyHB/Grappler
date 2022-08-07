using System.Collections.Generic;
using UnityEngine;

public class TwoPointRope : Rope
{
    public Transform StartPoint;
    public Transform EndPoint;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        ResetRope(StartPoint.position);
    }
   
    // Update is called once per frame
    void Update()
    {
        if (SegmentFrequency != segmentFrequency || LineLength != lineLength)
        {
            ResetRope(StartPoint.position);
        }

        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();
    }



    protected override void ApplyConstraint()
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
}