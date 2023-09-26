using UnityEngine;

/// <summary>
/// Destroy this projectile when colliding with another object
/// </summary>
public class DestroyProjectileDeathBehaviour : MonoBehaviour, IProjectileDeathBehaviour
{
	public void HandleCollision(Projectile projectile)
	{
		Destroy(projectile.gameObject);
	}
}
