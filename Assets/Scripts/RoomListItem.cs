using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour {
    //a pointer to a lot of functions as such it has the form of a function
    //create a delgate that contains a reference to several functions that several functions can subscribe to and can be called as a parameter no matter what
    public delegate void JoinRoomDelegate(MatchInfoSnapshot match);
    private JoinRoomDelegate joinRoomCallback;

    [SerializeField]
    private Text roomNameText;

    private MatchInfoSnapshot match;

    public void setup(MatchInfoSnapshot matchTemp, JoinRoomDelegate joinRoomCallbackTemp)
    {
        match = matchTemp;
        joinRoomCallback = joinRoomCallbackTemp;

        roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
    }

    public void joinRoom()
    {
        joinRoomCallback.Invoke(match);
    }
}
