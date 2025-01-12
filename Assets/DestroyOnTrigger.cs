using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    public GameObject GameObject;
    private void OnTriggerEnter2D() {

        Destroy(GameObject);
        Destroy(this);
    }
}
