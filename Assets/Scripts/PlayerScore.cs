using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//another useless script as the scoreboard isn't working because of how database was updated and unity 2018 but just keeping it in for completion sake
[RequireComponent(typeof(PlayerManager))]
public class PlayerScore : MonoBehaviour {

    int lastKills = 0;
    int lastDeaths = 0;
    
    PlayerManager player; 

	void Start () {
        player = GetComponent<PlayerManager>();
        StartCoroutine(syncScoreLoop());
	}

    void OnDestroy()
    {
        if(player != null)
        {
            syncNow();
        }
    }

    IEnumerator syncScoreLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            syncNow();
        }
    }

    void syncNow()
    {
        if (UserAccountManager.isLoggedIn)
        {
            UserAccountManager.instance.getData(onDataReceived);
        }
    }

    void onDataReceived(string data)
    {
        if(player.kills <= lastKills && player.deaths <= lastDeaths)
        {
            return;
        }

        int killsTemp = player.kills = lastKills;
        int deathsTemp = player.deaths = lastDeaths;

        int kills = DataParser.dataToKills(data);
        int deaths = DataParser.dataToDeaths(data);

        int newKillCount = killsTemp + kills;
        int newDeathCount = deathsTemp + deaths;

        string newData = DataParser.valuesToData(newKillCount, newDeathCount);

        Debug.Log("Syncing: " + newData);

        lastKills = player.kills;
        lastDeaths = player.deaths;

        UserAccountManager.instance.sendData(newData);
    }
}
