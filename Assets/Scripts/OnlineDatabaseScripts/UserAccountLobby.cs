using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserAccountLobby : MonoBehaviour {

    public Text usernameText;

    void Start()
    {
        if (UserAccountManager.isLoggedIn)
        {
            usernameText.text = UserAccountManager.loggedIn_Username;
        }
    }
    public void logOut()
    {
        if (UserAccountManager.isLoggedIn)
        {
            UserAccountManager.instance.logOut();
        }
    }
}
