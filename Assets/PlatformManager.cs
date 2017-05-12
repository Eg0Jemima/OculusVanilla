using UnityEngine;
using Oculus.Avatar;
using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections;
using UnityEngine.UI;

public class PlatformManager : MonoBehaviour
{
    public OvrAvatar myAvatar;
    private ulong roomId;
    public Text myText;

    void Awake()
    {
        Oculus.Platform.Core.Initialize();
        Oculus.Platform.Entitlements.IsUserEntitledToApplication().OnComplete(Entitled);
        Oculus.Platform.Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
        Oculus.Platform.Request.RunCallbacks();  //avoids race condition with OvrAvatar.cs Start().
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            RoomOptions myRoomOptions = new RoomOptions();
            Rooms.CreateAndJoinPrivate2(RoomJoinPolicy.Everyone, 2, myRoomOptions).OnComplete(RoomSetup);
        }
    }

    private void GetLoggedInUserCallback(Message<User> message)
    {
        if (!message.IsError)
        {
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
            string roomIdString = "My room ID = " + message.GetRoom().ID + "\n";
            myText.text += roomIdString;
            roomId = message.GetRoom().ID;
            Room myRoom = message.GetRoom();
            Debug.Log("Total users in room = " + myRoom.Users.Count);
            myText.text += "Total users in room " + myRoom.Users.Count + "\n";
            Rooms.GetInvitableUsers2().OnComplete(GetPeople);
        }
    }

    private void Entitled(Message message)
    {
        if (!message.IsError)
        {
            Debug.Log("This user is entitled");
            myText.text += "This user is entitled\n";
        }
        else
        {
            Debug.Log("Error = " + message.GetError().Message);
            myText.text += "Error = " + message.GetError().Message;
        }
    }

    private void UserJoined(Message message)
    {
        if (!message.IsError)
        {
            Debug.Log("Joined " + message.GetRoom().ID);
            Rooms.SetRoomInviteNotificationCallback((Message<string> msg) => {
                if (msg.IsError)
                {
                    // Handle error  
                }
                else
                {
                    string roomID = msg.GetString();
                    myText.text += msg.GetUser().ID + " joined room " + msg.GetRoom().ID;
                }
            }
            );
        }
        else
        {
            Debug.Log("Error = " + message.GetError().Message);
        }
    }

    private void GetPeople(Message<UserList> message)
    {
        if (!message.IsError)
        {
            Debug.Log("how many can I invite? " + message.Data[0].OculusID);
            myText.text += "how many can I invite? " + message.Data.Count + ", " + message.Data[0].OculusID + "\n";
            Rooms.InviteUser(roomId, message.Data[0].InviteToken).OnComplete(UserJoined);
        }
        else
        {
            Debug.Log("Error = " + message.GetError().Message);
        }
    }
}