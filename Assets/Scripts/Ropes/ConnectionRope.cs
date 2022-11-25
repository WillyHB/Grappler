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
            StartOffset = value;

            distanceJoint.anchor = transform.InverseTransformPoint(transform.position + (Vector3)startOffset);
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

    public bool IsStretched(float threshold = 0.01f)
    {
        return Mathf.Abs(GetLength() - GetDistanceBetweenBodies()) < threshold;
    }

    private DistanceJoint2D distanceJoint;
        
    public Vector2 GetRotatedPoint(Vector2 point, float eulerRotation)
    {
        float radians = eulerRotation * Mathf.Deg2Rad;

        float xRotate = point.x * (Mathf.Cos(radians)) - point.y * (Mathf.Sin(radians));
        float yRotate = point.x * (Mathf.Sin(radians)) + point.y * (Mathf.Cos(radians));

        return new Vector2(xRotate, yRotate);
    }
    public Vector2 GetCalculatedEndPoint()
    {
        Vector2 rotate = GetRotatedPoint(endOffset, connectedBody.rotation);

        return new Vector2(rotate.x + connectedBody.transform.position.x, rotate.y + connectedBody.transform.position.y);
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

        ResetRope(StartRigidbody.transform.position);
    }

    private void OnEnable()
    {
        distanceJoint.enabled = true;
        LineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        distanceJoint.enabled = false;
        LineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {     
        if (NumberOfSegments != numberOfSegments)
        {
            ModifyLength(GetCalculatedEndPoint());

        }

        DrawRope();
    }
    public float GetDistanceBetweenBodies() 
        => (((Vector2)StartRigidbody.transform.position + startOffset) - GetCalculatedEndPoint()).magnitude;


    private void FixedUpdate()
    {

        Simulate();

        distanceJoint.distance = GetLength();
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
