using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 6;

    private string roomName;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }
    public void setRoomName (string name)
    {
        roomName = name;
    }

    public void createRoom()
    {
        if (roomName != null && roomName != "")
        {
            Debug.Log("Creating room: " + roomName + " with room for " + roomSize + " players.");
            //create room look up parameters on unity documentation if curious although this is going to be deprecated soon
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }

}
