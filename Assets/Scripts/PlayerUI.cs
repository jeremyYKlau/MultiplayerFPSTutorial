using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform fuelAmount;
    [SerializeField]
    GameObject pauseMenu;

    private PlayerController playerController;

    void Start()
    {
        PauseMenu.isOn = false;    
    }

    public void setController(PlayerController controller)
    {
        playerController = controller;
    }

    void Update()
    {
        setFuelAmount(playerController.getThrusterFuelAmount());
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePauseMenu();
        }
    }

    void togglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }

    void setFuelAmount(float amount)
    {
        fuelAmount.localScale = new Vector3(1f, amount, 1f);
    }
}
