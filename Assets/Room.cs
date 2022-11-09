using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Room : MonoBehaviour
{
    public static Room ActiveRoom { get; private set; }


    public CinemachineVirtualCamera[] VirtualCameras;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {    
            if (ActiveRoom != null) ActiveRoom.Leave();
            ActiveRoom = this;
            ActiveRoom.Enter();
        }
    }
    

    private IEnumerator Wait()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>().Freeze();
        yield return new WaitForSecondsRealtime(Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>().UnFreeze();
    }

    public void Leave()
    {
        for (int i = 0; i < VirtualCameras.Length; i++) VirtualCameras[i].gameObject.SetActive(false);
    }

    public void Enter()
    {
        StartCoroutine(Wait());
        for (int i = 0; i < VirtualCameras.Length; i++) VirtualCameras[i].gameObject.SetActive(true);
    }
}
