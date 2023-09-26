using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSystemDebug : MonoBehaviour
{
    // References
    private Weapon weapon;
    private WeaponsController weaponsController;
    private AmmoController ammoController;
    
    public TextMeshProUGUI currentWeaponUI;
    public TextMeshProUGUI weaponStateUI;
    public TextMeshProUGUI boltsCountUI;
    public TextMeshProUGUI loadedUI;
    
    private void Start()
    {
        var playerGameObject = GameObject.FindWithTag("Player");
        weaponsController = playerGameObject.GetComponent<WeaponsController>();
        ammoController = playerGameObject.GetComponent<AmmoController>();
    }

    private void Update()
    {
        Weapon currentWeapon = weaponsController.GetCurrentWeapon();
        currentWeaponUI.text = "Current Weapon: " + weaponsController.GetCurrentWeapon().name;
        weaponStateUI.text = "Weapon State: " + weaponsController.GetCurrentWeapon().GetCurrentWeaponState().GetStateName(); // Holy function chain
        boltsCountUI.text = "Bolts: " + ammoController.Bolts.ToString("F0");
        if (currentWeapon.GetType() == typeof(Crossbow))
        {
            // The current weapon is a ranged weapon
            Crossbow crossbow = (Crossbow)currentWeapon;
            loadedUI.text = "Loaded?: " + crossbow.loaded;
        }
        else
        {
            loadedUI.text = "Loaded?: N/A";
        }

    }
}
