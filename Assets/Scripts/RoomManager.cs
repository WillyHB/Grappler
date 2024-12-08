using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private List<Room> rooms;

    public Room GetRoom(int i) {
        if (i < 0 || i > rooms.Count) return null;

        return rooms[i];
    }

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
