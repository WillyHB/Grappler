using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToPixelGrid : MonoBehaviour
{
    public Vector2 PixelGrid = new Vector2(16, 16);

    public Vector2 FloatPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(
            transform.position.x - (transform.position.x % (1.0f / 16)),
             transform.position.y - (transform.position.y % (1.0f / 16)),
              transform.position.z - (transform.position.z % (1.0f / 16)));
    }
}
