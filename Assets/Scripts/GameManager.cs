﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //for toarray and has a lot of other useful methods

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject sceneCamera;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;//used when we want to call multiple methods from one place, that's the purpose of a delegate callback

    //creates a singleton of the gamemanager because we will only ever have one
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one game manager in scene");
        }
        else
        {
            instance = this;
        }
    }

    public void setSceneCamera(bool isActive)
    {
        if(sceneCamera == null)
        {
            return;
        }
        sceneCamera.SetActive(isActive);
    }

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();
	
    public static void registerPlayer(string netID, PlayerManager player)
    {
        string playerId = PLAYER_ID_PREFIX + netID;
        players.Add(playerId, player);
        player.transform.name = playerId;
    }

    public static void unRegisterPlayer(string playerID)
    {
        //remove a player from the dictionary players based off playerID
        players.Remove(playerID);
    }

    public static PlayerManager getPlayer(string playerID)
    {
        return players[playerID];
    }

    public static PlayerManager[] getAllPlayers()
    {
        return players.Values.ToArray();
    }
    //unnessesary code just for a gui for the dictionary to help visualize in unity inspector
    /*void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200,200,200,500));
        GUILayout.BeginVertical();

        foreach (string playerID in players.Keys)
        {
            GUILayout.Label(playerID + " - " + players[playerID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }*/
    #endregion
}
