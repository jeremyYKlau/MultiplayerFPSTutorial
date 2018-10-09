using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;
    public bool isReloading = false;

	void Start () {
        equipWeapon(primaryWeapon);
	}

    public PlayerWeapon getCurrentWeapon()
    {
        return currentWeapon;
    }
    public WeaponGraphics getCurrentGraphics()
    {
        return currentGraphics;
    }

    void equipWeapon (PlayerWeapon weapon)
    {
        currentWeapon = weapon;

        GameObject weaponInstance = (GameObject)Instantiate(weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        weaponInstance.transform.SetParent(weaponHolder);

        currentGraphics = weaponInstance.GetComponent<WeaponGraphics>();
        if(currentGraphics== null)
        {
            Debug.LogError("No weaponsgraphics component on weapon: " + weaponInstance.name);
        }

        if (isLocalPlayer)
        {
            Util.setLayerRecursively(weaponInstance, LayerMask.NameToLayer(weaponLayerName));
        }
	}

    public void reload()
    {
        if (isReloading)
        {
            return;
        }
        StartCoroutine(ReloadAction());
    }

    private IEnumerator ReloadAction()
    {
        Debug.Log("Reloading...");

        isReloading = true;

        CmdOnReload();

        yield return new WaitForSeconds(currentWeapon.reloadTime);

        currentWeapon.currentBullets = currentWeapon.maxBullets;

        isReloading = false;
    }

    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload()
    {
        Animator anim = currentGraphics.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }

}
