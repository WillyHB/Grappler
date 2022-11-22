using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float Magnitude;

    private Vector3 startPos;

    public float XDampening = 1;

    public float YDampening = 1;

    public Vector2 Offset;

    // Start is called before the first frame update
    void Start()
    {
        startPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 camPos = new(Camera.main.transform.position.x * (Magnitude * XDampening), Camera.main.transform.position.y * (Magnitude * YDampening));

        transform.position = new Vector3(Camera.main.transform.position.x + Offset.x, Camera.main.transform.position.y + Offset.y, transform.position.z);

        GetComponent<SpriteRenderer>().material.SetVector("offset", new Vector4(camPos.x - startPos.x, camPos.y - startPos.y, 0, 0));


    }
}
