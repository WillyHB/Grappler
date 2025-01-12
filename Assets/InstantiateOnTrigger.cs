using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstantiateOnTrigger : MonoBehaviour
{
    public GameObject GameObject;
    
    private void OnTriggerEnter2D() 
    {
        Instantiate(GameObject);
        Destroy(this);
    }
}
