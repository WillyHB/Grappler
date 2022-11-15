using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Room> rooms;

    public void Start()
    {
        Room.RoomEntered += (room) =>
        {
            SaveObject so = GameData.Load();
            so.Checkpoint = rooms.IndexOf(room);
            GameData.Save(so);
        };
    }

    private void OnDisable()
    {
        Room.RoomEntered -= (room) =>
        {
            SaveObject so = GameData.Load();
            so.Checkpoint = rooms.IndexOf(room);
            GameData.Save(so);
        };
    }
}
