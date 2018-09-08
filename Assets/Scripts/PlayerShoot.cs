using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";
    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if(cam == null)
        {
            Debug.Log("Player shoot failed, no camera found");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot(); 
        }
    }

    //client methods only on the player/each client
    [Client]
    void shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            if(hit.collider.tag == PLAYER_TAG) //could also have used layer remote player
            {
                CmdPlayerShot(hit.collider.name, weapon.damage);
            }
        }
    }

    //called on server side and for protocal use cmd before each method to signify server side method
    [Command]
    void CmdPlayerShot(string playerID, int damage)
    {
        Debug.Log(playerID + " has been shot");

        PlayerManager player = GameManager.getPlayer(playerID);
        player.takeDamage(damage);
    }
}
