using UnityEngine;


public class GhoulAttackState : MonsterBaseState
{
	private Ghoul ghoul;
	
	public override void OnStateEnter(Monster monster)
	{
		// Cache our casted ghoul object
		ghoul = (Ghoul)monster;

		// Play our attack animation - damage is applied via an animation event
		ghoul.GetMonsterAnimator().SetTrigger("attack");
	}

	public override void OnStateTick(Monster monster)
	{
		// Wait until attack animation is done then go back to chase state
		if (!ghoul.GetMonsterAnimator().GetCurrentAnimatorStateInfo(0).IsName("Attack") && !ghoul.GetMonsterAnimator().IsInTransition(0))
		{
			// If we are not in attack state (we trigger it in state enter) that means the animation has finished
			monster.ChangeState(new GhoulChaseState());
		}
	}

	public override void OnStateExit(Monster monster)
	{

	}
}
