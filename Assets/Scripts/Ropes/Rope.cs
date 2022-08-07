using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public LayerMask CollisionLayerMask;
    public ContactFilter2D contactFilter = new() { useTriggers = false };

    RaycastHit2D[] RaycastHitBuffer = new RaycastHit2D[10];
    Collider2D[] ColliderHitBuffer = new Collider2D[10];

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

            Vector3 direction = (ropeSegments[i].posNow - firstSegment.posNow);

            int result = Physics2D.CircleCast(ropeSegments[i].posNow, 0.1f, direction.normalized, contactFilter, RaycastHitBuffer, direction.magnitude);
            
            if (result > 0)
            {
                for (int j = 0; j < result; j++)
                {
                    if (RaycastHitBuffer[j].collider.gameObject.layer == 3)
                    {
                        Vector2 hitPoint2 = RaycastHitBuffer[j].collider.bounds.ClosestPoint(RaycastHitBuffer[j].point);
                        Vector2 collidercenter = new Vector2(RaycastHitBuffer[j].collider.transform.position.x, RaycastHitBuffer[j].collider.transform.position.y);
                        Vector2 collisionDirection = RaycastHitBuffer[j].point - collidercenter;
                        Vector2 hitPos = collidercenter + collisionDirection;
                        firstSegment.posNow = hitPoint2;
                    }
                }
            }
            ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < ConstraintAppliedPerFrame; i++)
        {
            ApplyConstraint();

            AdjustCollision();
        }
    }

    protected void AdjustCollision()
    {
        
    }

    protected abstract void ApplyConstraint();

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
