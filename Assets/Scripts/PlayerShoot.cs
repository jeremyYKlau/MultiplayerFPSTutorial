using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

    void Start()
    {
        if(cam == null)
        {
            Debug.Log("Player shoot failed, no camera found");
            this.enabled = false;
        }
        weaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        currentWeapon = weaponManager.getCurrentWeapon();

        if (PauseMenu.isOn)
        {
            return;
        }

        if(currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("shoot", 0f, 1f/currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("shoot");
            }
        }

    }

    //called on server when a player shoots
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    //called on server when we hit something, takes hit point and normal of the surface
    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }

    //a method called on all individual clients when we need to do a shoot effect
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.getCurrentGraphics().muzzleFlash.Play();
    }

    //called on all clients, here we spawn effects of anysort
    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        //object pooling is much more effective than this
        GameObject hitEffect = (GameObject)Instantiate(weaponManager.getCurrentGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));//quaternion is to make sure effect comes out straight from the surface
        Destroy(hitEffect, 2f);
    }

    //client methods only on the player/each client
    [Client]
    void shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //shooting so call onshoot method on server
        CmdOnShoot();
        Debug.Log("SHOOTING!");//debug statement

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            if(hit.collider.tag == PLAYER_TAG) //could also have used layer remote player
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }
            //After hitting something check call onhit method on server
            CmdOnHit(hit.point, hit.normal);
        }
    }

    //called on server side and for protocal use cmd before each method to signify server side method
    [Command]
    void CmdPlayerShot(string playerID, int damage)
    {
        Debug.Log(playerID + " has been shot");

        PlayerManager player = GameManager.getPlayer(playerID);
        player.RpcTakeDamage(damage);
    }
}
