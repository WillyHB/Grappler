using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TwoPointRope : Rope
{
    public Transform StartPoint;
    public Transform EndPoint;


    void Start()
    {
        ResetRope(StartPoint.position);
    }
   
    // Update is called once per frame
    void Update()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = StartPoint.localPosition;
        ropeSegments[0] = firstSegment;

        RopeSegment endSegment = ropeSegments[^1];
        endSegment.posNow = EndPoint.localPosition;
        ropeSegments[^1] = endSegment;

        if (NumberOfSegments != numberOfSegments)
        {
            ModifyLength(EndPoint.position);
        }


        DrawRope();
    }

    public float GetDistanceBetweenPoints() => (StartPoint.position - EndPoint.position).magnitude;

    private void FixedUpdate()
    {
        Simulate();
    }

    protected override void ApplyConstraint()
    {
        for (int i = 0; i < NumberOfSegments - 1; i++)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = dist - LengthBetweenSegments;

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
}