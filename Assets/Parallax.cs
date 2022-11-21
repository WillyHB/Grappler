using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Range(0, 1)]
    public float Magnitude;

    private Vector3 startPos;

    [Range(0, 1)]
    public float XDampening = 1;
    [Range(0, 1)]
    public float YDampening = 1;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 camPos = new(Camera.main.transform.position.x * (Magnitude * XDampening), Camera.main.transform.position.y * (Magnitude * YDampening));

        transform.position = startPos + (Vector3)camPos;
    }
}
