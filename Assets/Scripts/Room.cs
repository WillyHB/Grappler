using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;


public class Room : MonoBehaviour
{
    public static Room ActiveRoom { get; private set; }
    public static event Action<Room> RoomEntered;
    public Transform Checkpoint;

    public RoomTraversalInputStateHandler RoomTraversalInputStateHandler;


    public CinemachineVirtualCamera[] VirtualCameras;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {    
            if (ActiveRoom != null) ActiveRoom.Leave();
            ActiveRoom = this;

            ActiveRoom.Enter();
            RoomEntered?.Invoke(ActiveRoom);
        }
    }
    

    private IEnumerator Wait()
    {
        RoomTraversalInputStateHandler.TraversingRoom = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>().Freeze();
        yield return new WaitForSeconds(Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>().UnFreeze(true);
        RoomTraversalInputStateHandler.TraversingRoom = false;
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
