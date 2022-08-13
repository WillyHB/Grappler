using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform Player;
    public float SmoothSpeed;

    public Vector3 FloatPosition { get; private set; }

    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {

        FloatPosition = Vector3.Lerp(FloatPosition, new Vector3(Player.position.x, Player.position.y, -10), SmoothSpeed);

        transform.position = FloatPosition;
        
        transform.position = new Vector3(
            FloatPosition.x - (FloatPosition.x % (1.0f / 16)),
             FloatPosition.y - (FloatPosition.y % (1.0f / 16)),
              FloatPosition.z - (FloatPosition.z % (1.0f / 16)));
    }
}
