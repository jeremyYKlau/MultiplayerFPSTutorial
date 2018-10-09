using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameplate : MonoBehaviour {

    [SerializeField]
    private Text usernameText;

    [SerializeField]
    private RectTransform healthbarFill;

    [SerializeField]
    private PlayerManager player;
	
	void Update () {
        usernameText.text = player.username;
        healthbarFill.localScale = new Vector3(player.getHealthPercent(), 1f, 1f);
	}
}
