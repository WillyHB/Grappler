using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEditor;


public class Room : MonoBehaviour
{
    public static Room ActiveRoom { get; private set; }
    public static event Action<Room> RoomEntered;
    public Transform Checkpoint;
    public float GlobalLighting = 0;
    private static int blendingSteps = 50;

    public RoomTraversalInputStateHandler RoomTraversalInputStateHandler;


    public CinemachineVirtualCamera[] VirtualCameras;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {    
            if (ActiveRoom != null) ActiveRoom.Leave();
            ActiveRoom = this;

            ActiveRoom.Enter(RoomManager.Instance.GetCheckpoint(this) == GameData.Load().checkpoint ? false : true);
            RoomEntered?.Invoke(ActiveRoom);
        }
    }
    

    private IEnumerator Wait(bool freezePlayer)
    {
        RoomTraversalInputStateHandler.TraversingRoom = true;
        if (freezePlayer) RoomManager.Instance.PlayerEventChannel.Freeze(true);
        yield return new WaitForSeconds(Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);
        if (freezePlayer) RoomManager.Instance.PlayerEventChannel.UnFreeze(true);
        // THIS Grrr
        RoomTraversalInputStateHandler.TraversingRoom = false;
    }
    
    private IEnumerator BlendLight()
    {

        Light2D globalLight = GameObject.Find("Global Light").GetComponent<Light2D>();
        float oldIntensity = globalLight.intensity;

        for (float i = 0; i < blendingSteps; i++) {

            globalLight.intensity = Mathf.Lerp(oldIntensity, GlobalLighting, i/blendingSteps);
            yield return new WaitForSeconds(Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time/blendingSteps);
        }
    }

    public void Leave()
    {
        for (int i = 0; i < VirtualCameras.Length; i++) VirtualCameras[i].gameObject.SetActive(false);
    }

    public void Enter(bool freezePlayer = true)
    {
        StartCoroutine(Wait(freezePlayer));
        StartCoroutine(BlendLight());
        for (int i = 0; i < VirtualCameras.Length; i++) VirtualCameras[i].gameObject.SetActive(true);
    }
}
