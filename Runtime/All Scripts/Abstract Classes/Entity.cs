using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for entities -> basically anything that can take damage
/// The player, enemy monsters, scene objects that can be destroyed like fences, boxes etc.
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] private float health;
    
    
    
    // Health property
    public virtual float Health
    {
        get { return health; }
        set
        {
            health = value;
            
            if (health <= 0)
            {
                Die();
            }
        }
    }
    
    // For derived classes to define
    public abstract void TakeDamage(float dmg);
    public abstract void Die();
}