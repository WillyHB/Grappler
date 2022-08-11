using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConnectionRope : Rope
{
    [Space(15)]
    [SerializeField] private Rigidbody2D StartRigidbody;
    [SerializeField] private Rigidbody2D ConnectedBody;

    [HideInInspector] public Rigidbody2D connectedBody 
    { 
        get => ConnectedBody; 

        set
        {
            distanceJoint.connectedBody = value;
            ConnectedBody = value;
        }
    }

    [Space(15)]
    [SerializeField]private Vector2 StartOffset;
    [HideInInspector]public Vector2 startOffset
    {
        get => StartOffset;

        set
        {
            distanceJoint.anchor = value;
            StartOffset = value;
        }
    }


    [SerializeField]private Vector2 EndOffset;
    [HideInInspector]
    public Vector2 endOffset
    {
        get => EndOffset;

        set
        {
            distanceJoint.connectedAnchor = value;
            EndOffset = value;
        }
    }

    private DistanceJoint2D distanceJoint;
        
    public Vector2 GetCalculatedEndPoint()
    {
        float rotation = connectedBody.rotation;

        float radians = rotation * Mathf.Deg2Rad;

        float xRotate = endOffset.x * Mathf.Cos(radians) - endOffset.y * Mathf.Sin(radians);
        float yRotate = endOffset.x * Mathf.Sin(radians) + endOffset.y * Mathf.Cos(radians);

        return new Vector2(xRotate + connectedBody.transform.position.x, yRotate + connectedBody.transform.position.y);
    }

    void Awake()
    {
        if (StartRigidbody.GetComponent<DistanceJoint2D>() == null)
        {
           StartRigidbody.gameObject.AddComponent<DistanceJoint2D>();
        }

        distanceJoint = StartRigidbody.GetComponent<DistanceJoint2D>();
        distanceJoint.connectedBody = ConnectedBody;
        distanceJoint.anchor = StartOffset;
        distanceJoint.connectedAnchor = EndOffset;
        distanceJoint.maxDistanceOnly = true;
        distanceJoint.enableCollision = true;
        lineRenderer = GetComponent<LineRenderer>();

        ResetRope(StartRigidbody.transform.position);
    }

    private void OnEnable()
    {
        distanceJoint.enabled = true;
        lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        distanceJoint.enabled = false;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SegmentFrequency != segmentFrequency || LineLength != lineLength)
        {
            ModifyLength(GetCalculatedEndPoint());
            //ResetRope(StartPoint.position);
        }

        DrawRope();
    }
    public float GetDistanceBetweenBodies() 
        => (((Vector2)StartRigidbody.transform.position + startOffset) - GetCalculatedEndPoint()).magnitude;

    private void FixedUpdate()
    {
        Simulate();

        distanceJoint.distance = LineLength;
    }

    protected override void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.posNow = (Vector2)StartRigidbody.transform.position + startOffset;
        ropeSegments[0] = firstSegment;

        //Constrant to Second Point 
        RopeSegment endSegment = ropeSegments[^1];
        endSegment.posNow = GetCalculatedEndPoint();
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
