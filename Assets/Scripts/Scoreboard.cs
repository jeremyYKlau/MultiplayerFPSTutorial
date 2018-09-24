using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {

    [SerializeField]
    GameObject playerScoreboardItem;

    [SerializeField]
    Transform playerScoreboardList;

    private void OnEnable()
    {
        PlayerManager[] players = GameManager.getAllPlayers();

        foreach(PlayerManager player in players)
        {
            GameObject itemGO = (GameObject)Instantiate(playerScoreboardItem, playerScoreboardList);
            PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
            if(item != null)
            {
                item.setup(player.username, player.kills, player.deaths);
            }
        }
    }

    private void OnDisable()
    {
        foreach (Transform child in playerScoreboardList)
        {
            Destroy(child.gameObject);
        }
    }
}
