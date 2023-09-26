using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Abstract base class for all monsters
/// </summary>
public class Monster : Entity
{
		// TODO: Scriptable object for monster stats?
		[SerializeField] protected string monsterName;
		[SerializeField] protected MonsterBaseState state;

		protected EnemyMovement movement;
		protected Animator animator;
		protected AudioSource audioSource;

		[SerializeField] protected AudioClip[] deathSounds;
		


		protected virtual void Awake()
		{
			movement = GetComponent<EnemyMovement>();
			animator = GetComponentInChildren<Animator>();
			audioSource = GetComponent<AudioSource>();
		}

		protected virtual void Update()
		{
			// Assign animator speed float
			animator.SetFloat("speed", movement.GetNavMeshAgent().velocity.magnitude);
			state.OnStateTick(this);
		}


		public void ChangeState(MonsterBaseState stateToTransitionTo)
		{
			if(state != null) state.OnStateExit(this);
			state = stateToTransitionTo;
			state.OnStateEnter(this);
			
		}
		
		public String GetMonsterName()
		{
			return monsterName;
		}

		public EnemyMovement GetMonsterMovementController()
		{
			return movement;
		}

		public Animator GetMonsterAnimator()
		{
			return animator;
		}

		public virtual MonsterBaseState GetMonsterState()
		{
			Debug.Log(state);
			return state;
		}

		public override void TakeDamage(float dmg)
		{
			// Death check is done in health property on entity
			Health -= dmg;
		}

		public override void Die()
		{
			// TODO: temporary stuff

			Destroy(this.gameObject);
		}

		
}
