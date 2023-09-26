using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// A class for a non-playable entity derived physics object in the scene. Think like a prop.
/// Thinks like buckets, stuff on the ground etc etc that can move around, interact with the player
/// and with gravity 
/// </summary>
public class SceneObject : Entity
{

    public override void TakeDamage(float dmg)
    {
        // Remove health - health is validated in entity
        Health -= dmg; // TODO: make sure this is calling this classes die function not entities.
    }

    public override void Die()
    {
        Destroy(this.gameObject);
    }
}
