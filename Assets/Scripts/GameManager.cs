using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

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
}
