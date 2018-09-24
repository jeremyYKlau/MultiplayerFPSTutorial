using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;
    [SerializeField]
    private GameObject roomListItemPrefab;
    [SerializeField]
    private Transform roomListParent;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
        refreshRoomList();
    }

    public void refreshRoomList()
    {
        clearRoomList();
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, onMatchList);
        status.text = "Loading...";
    }

    public void onMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";
        if (!success || matchList == null)
        {
            status.text = "Couldn't get room list";
            return;
        }
        foreach (MatchInfoSnapshot match in matchList)
        {
            GameObject roomListItemGO = Instantiate(roomListItemPrefab);
            roomListItemGO.transform.SetParent(roomListParent);
            
            RoomListItem roomListItem = roomListItemGO.GetComponent<RoomListItem>();
            if(roomListItem != null)
            {
                roomListItem.setup(match, joinRoom);
            }
            roomList.Add(roomListItemGO);
        }
        if(roomList.Count == 0)
        {
            status.text = "No rooms at the moment.";
        }
    }

    void clearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }

    public void joinRoom (MatchInfoSnapshot match)
    {
        networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
    }

    IEnumerator WaitForJoin()
    {
        clearRoomList();

        int countDown = 10; //set this to 10 seconds at least for a time out check, its good practice
        while (countDown > 0)
        {
            status.text = "Joining... (" + countDown + ")";

            yield return new WaitForSeconds(1);
            countDown--;
        }
        //if we reach down here we failed to connect because every other option would go somewhere else in the code
        status.text = "Failed to connect.";
        yield return new WaitForSeconds(1);

        MatchInfo matchInfo = networkManager.matchInfo;
        if(matchInfo != null)
        {
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }
        refreshRoomList();
    }
}
