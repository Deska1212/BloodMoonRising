using UnityEngine;

public class ProjectileDamageBehaviour : MonoBehaviour, IProjectileDamageBehaviour
{
	public void ApplyDamage(IDamageable damageable, float damageToApply)
	{
		damageable.TakeDamage(damageToApply);
	}
}