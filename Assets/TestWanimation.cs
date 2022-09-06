using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWanimation : MonoBehaviour
{
    public Wanimation wanimation;

    public void Start()
    {
        GetComponent<Wanimator>().Play(wanimation);
    }
}
