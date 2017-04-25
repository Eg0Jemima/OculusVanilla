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
        //Oculus.Platform.Rooms.CreateAndJoinPrivate2(RoomJoinPolicy.Everyone, 2, null);
        //Oculus.Platform.Matchmaking.Enqueue("EJRoom");
        //Oculus.Platform.Matchmaking.JoinRoom(Oculus.Platform.Rooms.GetCurrent().RequestID, false);
        Oculus.Platform.Request.RunCallbacks();  //avoids race condition with OvrAvatar.cs Start().
        
    }

    private void GetLoggedInUserCallback(Message<User> message)
    {
        if (!message.IsError) {
            myAvatar.oculusUserID = message.Data.ID;
        }
    }
}