using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform fuelAmount;
    [SerializeField]
    RectTransform healthAmount;
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject scoreboard;
    [SerializeField]
    Text ammoText;

    private PlayerManager player;
    private PlayerController playerController;
    private WeaponManager weaponManager;

    void Start()
    {
        PauseMenu.isOn = false;    
    }

    //done this way because the playermanager is the main component and playercontroller is part of it so this is better than just getting the playercontroller
    public void setPlayer(PlayerManager playerTemp)
    {
        player = playerTemp;
        playerController = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<WeaponManager>();
    }

    void Update()
    {
        setFuelAmount(playerController.getThrusterFuelAmount());
        setHealthAmount(player.getHealthPercent());
        setAmmoAmount(weaponManager.getCurrentWeapon().currentBullets, weaponManager.getCurrentWeapon().maxBullets);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePauseMenu();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    public void togglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }

    void setFuelAmount(float amount)
    {
        fuelAmount.localScale = new Vector3(1f, amount, 1f);
    }

    void setHealthAmount(float amount)
    {
        healthAmount.localScale = new Vector3(1f, amount, 1f);
    }

    void setAmmoAmount(int amount, int maxAmount)
    {
        ammoText.text = amount.ToString() + "/" + maxAmount.ToString();
    }
}
