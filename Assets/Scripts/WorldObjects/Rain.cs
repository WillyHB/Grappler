using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();


    }
}
