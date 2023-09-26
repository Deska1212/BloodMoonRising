using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a base class and defines behaviour for derived weapon classes
/// </summary>
public class Weapon : MonoBehaviour
{
    protected string weaponName; // Set by deriving classes
    protected WeaponBaseState state; // TODO: Do derived classes need to know about this? State is handled mostly in Weapon?

    public GameObject weaponModel; // Gameobject to apply scripted animations to so position isn't overridden by unity animations
    public Vector3 defaultPosition;
    
    // References
    protected WeaponsController weaponsController; // Weapons/Weapon switching logic
    protected Animator animator;

    private void Awake()
    {
        defaultPosition = transform.localPosition;
        Debug.Log(defaultPosition);
    }

    protected virtual void Update()
    {
        state.OnStateTick(this);
    }

    public void ChangeState(WeaponBaseState stateToTransitionTo)
    {
        if(state != null) state.OnStateExit(this);
        state = stateToTransitionTo;
        state.OnStateEnter(this);
    }

    /// <summary>
    /// Causes this weapon to transition to equipping state
    /// </summary>
    public virtual void Equip()
    {
        
    }

    /// <summary>
    /// Causes this weapon to transition to unequipping state
    /// </summary>
    public virtual void Unequip()
    {
        
    }

    /// <summary>
    /// Moves this weapon to its ready state
    /// </summary>
    public virtual void Ready()
    {
        
    }

    public Animator GetWeaponAnimator()
    {
        return animator;
    }

    public WeaponBaseState GetCurrentWeaponState()
    {
        return state;
    }

    public WeaponsController GetWeaponsController()
    {
        return weaponsController;
    }
}
