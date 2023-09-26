using UnityEngine;

/// <summary>
/// Abstraction to handle behaviour for this projectile when it collides with something
/// </summary>
public interface IProjectileDeathBehaviour
{
	public void HandleCollision(Projectile projectile);
}
