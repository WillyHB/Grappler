using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform Player;

    void Start()
    {
        //Time.fixedDeltaTime = 0.02f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Player.position.x, Player.position.y, -10);
    }
}
