using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnePointRopeTwo : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private readonly List<RopeSegment> ropeSegments = new List<RopeSegment>();

    private int numberOfSegments;

    [HideInInspector]public float LengthBetweenSegments = 0.25f;
    [HideInInspector] public int NumberOfSegments = 35;
    [HideInInspector] public int SegmentFrequency = 10;
    [HideInInspector] public int LineLength = 10;
    [HideInInspector] public float LineWidth = 0.1f;
    [HideInInspector] public bool AdvancedOptions = false;

    public int ConstraintAppliedPerFrame = 100;

    public float GravityScale = 1;

    public bool FollowMousePosition;

    // Use this for initialization
    void Start()
    {
        if (!AdvancedOptions)
        {
            NumberOfSegments = SegmentFrequency * LineLength;
            LengthBetweenSegments = (float)LineLength / NumberOfSegments;
        }

        lineRenderer = GetComponent<LineRenderer>();

        ResetRope();
    }

    private void ResetRope()
    {
        ropeSegments.Clear();
        numberOfSegments = NumberOfSegments;

        Vector3 ropeStartPoint = FollowMousePosition ? Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) : transform.position;

        for (int i = 0; i < numberOfSegments; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= LengthBetweenSegments;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (NumberOfSegments != numberOfSegments)
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
        //Constrant to Mouse
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = FollowMousePosition ? Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) : transform.position;
        ropeSegments[0] = firstSegment;

        for (int i = 0; i < numberOfSegments - 1; i++)
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

    private void DrawRope()
    {
        lineRenderer.startWidth = LineWidth;
        lineRenderer.endWidth = LineWidth;

        Vector3[] ropePositions = new Vector3[NumberOfSegments];
        for (int i = 0; i < NumberOfSegments; i++)
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
