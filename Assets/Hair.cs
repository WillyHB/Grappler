using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hair : MonoBehaviour
{
    public int NumberOfSegments;
    public float LengthBetweenSegments;

    public LineRenderer LineRenderer;
    public Rigidbody2D Rigidbody;

    private Vector3[] segmentsPositions;

    public Vector2 Offset;

    public float Length;

    // Start is called before the first frame update
    void Start()
    {
        segmentsPositions = new Vector3[NumberOfSegments];

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 normalizedVelocity = Rigidbody.velocity.normalized;

        Vector2 dir = -new Vector2(normalizedVelocity.x, normalizedVelocity.y == 0 ? 1 : normalizedVelocity.y);

        segmentsPositions[0] = transform.position;

        segmentsPositions[1] = transform.position + (Vector3)dir * Length;

        LineRenderer.SetPositions(segmentsPositions);


    }
}
