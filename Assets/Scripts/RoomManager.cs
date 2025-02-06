using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private List<Room> rooms;

    public static RoomManager Instance {get; private set;}

    public Room GetRoom(int i) 
    {
        if (i < 0 || i > rooms.Count) return null;

        return rooms[i];
    }

    public int GetCheckpoint(Room room) 
    {
        if (!rooms.Contains(room)) return -1;
        return rooms.IndexOf(room);
    }

    public void Start()
    {
        Instance = this;

        Room.RoomEntered += (room) =>
        {
            SaveObject so = GameData.Load();
            so.checkpoint = rooms.IndexOf(room);
            GameData.Save(so);
        };
    }

    private void OnDisable()
    {
        Room.RoomEntered -= (room) =>
        {
            SaveObject so = GameData.Load();
            so.checkpoint = rooms.IndexOf(room);
            GameData.Save(so);
        };
    }
}
