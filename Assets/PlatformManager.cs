using UnityEngine;
using Oculus.Avatar;
using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections;

public class PlatformManager : MonoBehaviour
{

    public OvrAvatar myAvatar;
    private ulong roomId;

    void Awake()
    {
        Oculus.Platform.Core.Initialize();
        Oculus.Platform.Entitlements.IsUserEntitledToApplication().OnComplete(Entitled);
        Oculus.Platform.Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
        Oculus.Platform.Request.RunCallbacks();  //avoids race condition with OvrAvatar.cs Start().
        //Rooms.CreateAndJoinPrivate(RoomJoinPolicy.Everyone, 2, false).OnComplete(RoomSetup);
        Rooms.Join(127250654493430, false).OnComplete(JoinedRoom);
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
            roomId = message.GetRoom().ID;
            Room myRoom = message.GetRoom();
            Debug.Log("Total users in room = " + myRoom.Users.Count);
            Rooms.GetInvitableUsers().OnComplete(GetPeople);
        }
    }

    private void Entitled(Message message)
    {
        if (!message.IsError)
        {
            Debug.Log("This user is entitled to the app now");
        } else
        {
            Debug.Log("Error = " + message.GetError().Message);
        }
    }

<<<<<<< HEAD
    private void GetPeople(Message<UserList> message)
    {
        if (!message.IsError)
        {
            Debug.Log("how many can I invite? " + message.Data[0].OculusID);
            Rooms.InviteUser(roomId, message.Data[0].InviteToken);
        }
        else
        {
            Debug.Log("Error = " + message.GetError().Message);
=======
    //Playing around with Oculus Rooms
    private void JoinedRoom(Message<Room> message)
    {
        if (!message.IsError)
        {
            Debug.Log("My Appplication ID = " + message.GetRoom().ApplicationID);
            Room myRoom = message.GetRoom();
            Debug.Log("Total users in room = " + myRoom.Users.Count);
>>>>>>> 0862be518cebc2c55b1cddd067b685647e746160
        }
    }
}