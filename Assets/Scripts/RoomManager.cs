using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Room> rooms;

    public void Start()
    {
        Room.RoomEntered += (room) => PlayerPrefs.SetInt("Checkpoint", rooms.IndexOf(room));
    }

    private void OnDisable()
    {
        Room.RoomEntered -= (room) => PlayerPrefs.SetInt("Checkpoint", rooms.IndexOf(room));
    }
}
