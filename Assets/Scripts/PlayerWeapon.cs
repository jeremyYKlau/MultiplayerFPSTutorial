using UnityEngine;

[System.Serializable]//without tag this will not show up as a variable in inspector when using it for weapon
public class PlayerWeapon {

    public string name = "Weapon";

    public int damage = 10;
    public float range = 100f;
}
