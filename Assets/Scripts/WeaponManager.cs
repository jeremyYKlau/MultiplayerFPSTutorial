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
}
