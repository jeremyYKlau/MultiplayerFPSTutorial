using UnityEngine;

[System.Serializable]//without tag this will not show up as a variable in inspector when using it for weapon
public class PlayerWeapon {

    public string name = "Weapon";

    public int damage = 10;
    public float range = 100f;

    public float fireRate = 0f;

    public int maxBullets = 20;
    [HideInInspector]
    public int currentBullets;

    public float reloadTime = 1f;

    public GameObject graphics;

    public PlayerWeapon()
    {
        currentBullets = maxBullets;
    }
}
