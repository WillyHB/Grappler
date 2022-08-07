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

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = dist - lengthBetweenSegments;
            Vector2 changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;

            Vector2 changeAmount = changeDir * error;


            
            int result = Physics2D.CircleCast(ropeSegments[i].posNow, 0.1f, changeAmount.normalized, contactFilter, RaycastHitBuffer, changeAmount.magnitude);

            if (result > 0)
            {
                for (int j = 0; j < result; j++)
                {
                    if (RaycastHitBuffer[j].collider.gameObject.layer == 3)
                    {
                        /*
                         * 
                         * WE NEED TO RETHINK OUR MATH
                         * THE CONSTRAINTS ARE PULLING THE ROPE INTO THE COLLIDER
                         * IF THERE IS SLACK (WHICH THERE IS ON ONE POINT ROPES) THIS SHOULD NOT OCCUR
                         * 
                         * WE SHOULD MOVE POINTS BELOW TO AVOID THIS I DON'T KNOW MAN
                    }
                }
            }

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
