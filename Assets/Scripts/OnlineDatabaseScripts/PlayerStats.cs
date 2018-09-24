using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//currently this whole file is useless and can be removed if i can't get it to work later, also i don't really want to use it as i don't really want to save any data
public class PlayerStats : MonoBehaviour {

    public Text killCount;
    public Text deathCount;

	// Use this for initialization
	void Start () {
        if (UserAccountManager.isLoggedIn)
        {
            UserAccountManager.instance.getData(onRecievedData);
        }
    }
	void onRecievedData(string data)
    {
        //this code doesn't work probably delete when done tutorial as this has been deprecated
        if (killCount == null || deathCount == null)
            return;

        killCount.text = DataParser.dataToKills(data).ToString() + " KILLS";
        deathCount.text = DataParser.dataToDeaths(data).ToString() + " DEATHS";
    }
}
