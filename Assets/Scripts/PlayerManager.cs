using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class PlayerManager : NetworkBehaviour {

    [SyncVar]//variable synced across every client and server, have to do it this way below for a syncVar
    private bool _isDead = false;
    public bool isDead
    {
        get{return _isDead;}
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;    
    //syncs the variable across all the clients not just the server
    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    //need this in inspector to match the above behaviors enabled at the setup
    [SerializeField]
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    public void setupPlayer ()
    {
        if (isLocalPlayer)
        {
            //disable scene camera when spawned in 
            GameManager.instance.setSceneCamera(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadCastNewPlayerSetup();
    }
    //notifies server of the setup
    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }
    //sets up player defaults on only its own client
    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            //this works because we aren't reordering anything, loop through all components and store whether they are enabled or not in array
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }

        setDefaults();
    }
    /*just for debugging to test death of player instakill key when pressed K
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(9999);
        }
    }*/

    [ClientRpc]//a command that is sent through all clients to ensure everyone gets it
    public void RpcTakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= amount;
        Debug.Log(transform.name + " now has " + currentHealth + " health");
        if(currentHealth <= 0)
        {
            playerDie();
        }
    }

    private void playerDie()
    {
        isDead = true;

        //disable components for dead player
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        //disable gameobjects for dead player
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }
        //disable collider because unlike behavior components cannot be enable/disable but colliders can
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        //spawn a death effect, by the way we cast to game object so we can access the destroy method
        GameObject deathGraphics = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathGraphics, 3f);

        //switch cameras 
        if (isLocalPlayer)
        {
            GameManager.instance.setSceneCamera(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " IS DEAD");

        StartCoroutine(Respawn());
    }

    public void setDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        //opposite of for loop in setup method, enable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        //enable gameobjects for spawn or respawned player
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }
        //enable collider
        Collider collider = GetComponent<Collider>();
        if(collider != null)
        {
            collider.enabled = true;
        }
        //spawn effect
        GameObject spawnGraphics = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(spawnGraphics, 3f);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        //to ensure that we move the player before instantiating the particles
        yield return new WaitForSeconds(0.1f);

        setupPlayer();
        Debug.Log(transform.name + " respawned");
    }
}
