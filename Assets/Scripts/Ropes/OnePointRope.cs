using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnePointRope : Rope
{
    public bool FollowMousePosition;

    void Start()
    {
        ResetRope(FollowMousePosition ? Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) : Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = FollowMousePosition ? Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) : Vector2.zero;
        ropeSegments[0] = firstSegment;

        if (NumberOfSegments != numberOfSegments)
        {
            ModifyLength();

        }

        DrawRope();
    }

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

            // The distance between the two rope segments
            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;

            // The amount that the rope is stretched out of it's normal operating range
            float error = dist - LengthBetweenSegments;

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
}