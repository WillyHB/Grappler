using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public abstract class Rope : MonoBehaviour
{
    protected LineRenderer lineRenderer;

    protected readonly List<RopeSegment> ropeSegments = new List<RopeSegment>();

    [Header("Rope Settings")]

    public int NumberOfSegments = 35;

    protected int numberOfSegments;

    public float LengthBetweenSegments = 0.25f;

    public float LineWidth = 0.1f;

    public int ConstraintAppliedPerFrame = 100;

    public float GravityScale = 1;

    [Space(10)]
    [Header("Wind")]

    [Min(0f)]
    public float GustPowerFrom;
    [Min(0f)]
    public float GustPowerTo; 
    
    public Vector2 NormalizedWindDirection;

    [Space(10)]
    [Header("World Interaction")]

    public bool PushedByRigidbodies = false;

    public float PushMultiplier = 0.1f;

    protected void Simulate()
    {
        // SIMULATION
        for (int i = 1; i < NumberOfSegments; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;

            if (i != ropeSegments.Count - 1 && i != 0)
            {
                velocity += Random.Range(GustPowerFrom, GustPowerTo) * Time.deltaTime * NormalizedWindDirection.normalized;
            }

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


        // SET COLLIDER
        if (PushedByRigidbodies)
        {

            if (GetComponent<EdgeCollider2D>() == null)
            {
                gameObject.AddComponent<EdgeCollider2D>().isTrigger = true;
            }

            List<Vector2> points = new();
            ropeSegments.ForEach(seg => points.Add(transform.InverseTransformPoint(seg.posNow)));
            GetComponent<EdgeCollider2D>().SetPoints(points);
        }

        else
        {
            if (GetComponent<EdgeCollider2D>() != null)
            {
                Destroy(gameObject.GetComponent<EdgeCollider2D>());
            }

           
        }
    }

    protected abstract void ApplyConstraint();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PushedByRigidbodies)
        {
            return;
        }

        for (int i = 0; i < ropeSegments.Count; i++)
        {
            if (collision.bounds.Contains(transform.TransformPoint(GetComponent<EdgeCollider2D>().points[i])))
            {
                RopeSegment seg = ropeSegments[i];

                seg.posNow += collision.GetComponent<Rigidbody2D>().velocity.normalized * PushMultiplier;

                ropeSegments[i] = seg;
            }
        }
    }


    protected void ModifyLength(Vector2? pos = null)
    {
        numberOfSegments = NumberOfSegments;

        if (NumberOfSegments > 0 && LengthBetweenSegments > 0)
        {
            while (NumberOfSegments > ropeSegments.Count)
            {
                ropeSegments.Add(new RopeSegment(pos ?? new Vector2(ropeSegments[^1].posNow.x, ropeSegments[^1].posNow.y - LengthBetweenSegments)));
            }

            while (NumberOfSegments < ropeSegments.Count)
            {
                ropeSegments.RemoveAt(ropeSegments.Count - 1);
            }
        }
    }
    protected void ResetRope(Vector3 startPosition)
    {
        ropeSegments.Clear();

        if (NumberOfSegments > 0 && LengthBetweenSegments > 0)
        {
            Vector3 ropeStartPoint = startPosition;

            for (int i = 0; i < NumberOfSegments; i++)
            {
                ropeSegments.Add(new RopeSegment(ropeStartPoint));
                ropeStartPoint.y -= LengthBetweenSegments;
            }

        }
    }


    public float GetLength()
    {
        return NumberOfSegments * LengthBetweenSegments;
    }

    public void SetLength(float Length)
    {
        LengthBetweenSegments = Length / NumberOfSegments;
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

