using UnityEngine;

public interface IProjectileDamageBehaviour
{
	public void ApplyDamage(IDamageable damageable, float damageToApply);
}