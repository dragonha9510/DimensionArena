using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRoomInfo : MonoBehaviour
{
    private string roomName = "";
    public string RoomName { get { return roomName; } }
    private int playerCount = 0;
    public int PlayerCount { get { return playerCount; } }

    private bool isOpen = true;
    public bool IsOpen { get { return isOpen; } set { isOpen = value; } }

    public CustomRoomInfo(string room,int count)
    {
        roomName = room;
        playerCount = count;
    }
}
