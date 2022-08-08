using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnePointRope : Rope
{

    public bool FollowMousePosition;

    private RaycastHit2D[] RaycastHitBuffer = new RaycastHit2D[10];

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        ResetRope(FollowMousePosition ? Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) : transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (lineLength != LineLength || segmentFrequency != SegmentFrequency)
        {
            ResetRope(FollowMousePosition ? Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) : transform.position);
        }

        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();
    }

    protected override void ApplyConstraint()
    {
        //Constrant to Mouse
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = FollowMousePosition ? Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) : transform.position;
        ropeSegments[0] = firstSegment;

        for (int i = 0; i < numberOfSegments - 1; i++)
        {   
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            // The distance between the two rope segments
            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;

            // The amount that the rope is stretched out of it's normal operating range
            float error = dist - lengthBetweenSegments;

            // Normalized vector between the two segments
            Vector2 changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;

            // the vector that the rope segment has to move to pull itself back to normal operating range
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
