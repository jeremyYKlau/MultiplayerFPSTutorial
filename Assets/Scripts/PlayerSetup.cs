using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;
    [SerializeField]
    private string remoteLayerName = "RemotePlayer";
    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;
    [SerializeField]
    public GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIInstance;
    

    void Start()
    {
        if (!isLocalPlayer)
        {
            disableComponents();
            assignRemoteLayer();
        }
        else
        {
            //disable player graphics
            setLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //create playerUI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if(ui == null)
            {
                Debug.LogError("No ui component on player ui prefab");
            }
            ui.setController(GetComponent<PlayerController>());

            GetComponent<PlayerManager>().setupPlayer();

            string usernameTemp = "loading... ";
            if (UserAccountManager.isLoggedIn)
            {
                usernameTemp = UserAccountManager.loggedIn_Username;
            }
            else
            {
                usernameTemp = transform.name;
            }
            CmdSetUsername(transform.name, usernameTemp);
        }
    }

    [Command]
    void CmdSetUsername(string playerID, string username)
    {
        PlayerManager player = GameManager.getPlayer(playerID);
        if(player != null)
        {
            Debug.Log(username + " has joined");
            player.username = username;
        }
    }

    //recursive method, dangerous because if they don't function properly can get stuck at a state and freeze the game
    //if this doesn't function properly may cause errors and even game freeze/crash
    void setLayerRecursively (GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            setLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerManager player = GetComponent<PlayerManager>();
        GameManager.registerPlayer(netId, player);
    }

    void assignRemoteLayer()
    {
        gameObject.layer = 10;
        //gameObject.layer = LayerMask.NameToLayer(remoteLayerName); //doesn't work for some reason
    }

    void disableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }
    //when we are destroyed
    void OnDisable()
    {
        Destroy(playerUIInstance);

        if (isLocalPlayer)
        {
            GameManager.instance.setSceneCamera(true);
        }

        GameManager.unRegisterPlayer(transform.name);
    }
}
