using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowards : MonoBehaviour
{
    public Transform ObjectToFace;

    private void Update()
    {
        Vector3 dir = transform.position - ObjectToFace.position;

        Quaternion rot = Quaternion.LookRotation(Vector3.forward, -dir);

        transform.rotation = rot;
    }
}
