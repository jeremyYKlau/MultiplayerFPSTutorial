using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour {

    [SerializeField]
    Text text;

    public void setup(string player, string source)
    {
        //can use html tags to edit text objects like bold italic or even <color = red>
        text.text = "<b>" + source + "</b>" + " Killed " + "<i>" + player + "</i>";
    }
}
