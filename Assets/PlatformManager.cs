using UnityEngine;
using Oculus.Avatar;
using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections;

public class PlatformManager : MonoBehaviour
{

    public OvrAvatar myAvatar;

    void Awake()
    {
        Oculus.Platform.Core.Initialize();
        Oculus.Platform.Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
        Oculus.Platform.Request.RunCallbacks();  //avoids race condition with OvrAvatar.cs Start().
        Rooms.CreateAndJoinPrivate(RoomJoinPolicy.Everyone, 2, false).OnComplete(RoomSetup);
    }

    private void GetLoggedInUserCallback(Message<User> message)
    {
        if (!message.IsError) {
            myAvatar.oculusUserID = message.Data.ID;
            Debug.Log(message.Data.OculusID);
        }
    }

    //Playing around with Oculus Rooms
    private void RoomSetup(Message<Room> message)
    {
        if (!message.IsError)
        {
            Debug.Log("My room ID = " + message.GetRoom().ID);
            Room myRoom = message.GetRoom();
            Debug.Log("Total users in room = " + myRoom.Users.Count);
        }
    }
}