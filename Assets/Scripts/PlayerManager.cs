using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour {

    [SerializeField]
    private int maxHealth = 100;

    //syncs the variable across all the clients not just the server
    [SyncVar]
    private int currentHealth;

    void Awake()
    {
        setDefaults();
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(transform.name + " now has " + currentHealth + " health");
    }
    public void setDefaults()
    {
        currentHealth = maxHealth;
    }
}
