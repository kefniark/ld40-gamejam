using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Building;

using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;
using Assets.Scripts.Components.StateMachine;

namespace Assets.Scripts.Entities.Characters
{
	public abstract class BaseCharacter : MonoBehaviour
	{
		public event EventHandler InterestReached;
		public event EventHandler InterestChanged;
		public event EventHandler CharacterUpseted;

		public float MaxWait = 10;
		
		protected float LastDecision;
		private int Id;

		public CharacterConfig Config;
		public NavMeshAgent NavMeshAgent;
		public CharacterInterest Interest { get { return interest; } }
		private CharacterInterest interest = null;

		public StateMachine<CharacterActionEnum, BaseCharacter> States;
		private BaseBuilding Origin = null;
		private BaseBuilding Target = null;

		public void Setup(BaseBuilding house)
		{
			Id = Random.Range(0, 99999);
			Origin = house;

			States = new StateMachine<CharacterActionEnum, BaseCharacter>(this);
			States.Add(CharacterActionEnum.None, new State<CharacterActionEnum, BaseCharacter>());
			States.Add(CharacterActionEnum.WaitInside, new StateWaitInside());
			States.Add(CharacterActionEnum.LeaveAnimation, new StateLeaveAnimation());
			States.Add(CharacterActionEnum.EnterAnimation, new StateEnterAnimation());
			States.Add(CharacterActionEnum.Decide, new StateDecide());
			States.Add(CharacterActionEnum.WaitTarget, new StateWaitTarget());
			States.Add(CharacterActionEnum.MoveToTarget, new StateMoveToTarget());
			States.Add(CharacterActionEnum.LeaveTargetAnimation, new StateLeaveTargetAnimation());
			States.Add(CharacterActionEnum.EnterTargetAnimation, new StateEnterTargetAnimation());
			States.Add(CharacterActionEnum.MoveToHome, new StateMoveToHome());
			States.Start();

			// States.StateChanged += (sender, arg) => Debug.Log($"[CharacterState {States.Previous} => {States.Current}]");

			// Debug.Log($"{this} Setup");
			NavMeshAgent.enabled = true;
			States.ChangeState(CharacterActionEnum.WaitInside);
		}

		#region Decision

		public void SolvedInterest()
		{
			// Debug.LogWarning($"{this} Completed her need of {interest}");
			InterestReached?.Invoke(this, EventArgs.Empty);
			interest = null;
		}

		public void DecideInterest()
		{
			if (interest != null)
			{
				return;
			}
			
			interest = Config.PickInterest();
			LastDecision = Time.time;
			// Debug.LogWarning($"{this} Want {interest}");

			InterestChanged?.Invoke(this, EventArgs.Empty);
		}

		public void Upset()
		{
			// Debug.Log($"{this} Upseted");
			CharacterUpseted?.Invoke(this, EventArgs.Empty);
			interest = null;
		}

		#endregion

		#region Destination

		public bool AgentHasReachedDestination()
		{
			if (NavMeshAgent == null)
			{
				return false;
			}

			if (NavMeshAgent.pathPending)
			{
				return false;
			}

			if (!(NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance))
			{
				return false;
			}

			if (!NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude <= 0f)
			{
				return true;
			}

			return false;
		}

		protected BaseBuilding FindHome()
		{
			// Debug.Log($"{this} FindHome {Origin}");
			return Origin;
		}

		protected BaseBuilding FindTarget()
		{
			List<BaseBuilding> slots = Slot.Slots
				.Where(x => x.Building != null && x.Building.Type == interest.Type)
				.Where(x => Vector3.Distance(x.Building.transform.position, transform.position) < interest.Radius)
				.Select(x => x.Building)
				.ToList();
			if (slots.Count == 0)
			{
				return null;
			}

			var rnd = new System.Random();
			int r = rnd.Next(slots.Count);

			// Debug.Log($"{this} FindTarget {slots[r]}");
			return slots[r];
		}

		public BaseBuilding GoToHome()
		{
			Target = FindHome();
			if (Target != null)
			{
				NavMeshAgent.destination = Target.Door.transform.position;
			}
			return Target;
		}

		public BaseBuilding GoToTarget()
		{
			Target = FindTarget();
			if (Target != null)
			{
				NavMeshAgent.destination = Target.Door.transform.position;
			}
			return Target;
		}

		#endregion

		public void Update()
		{
			States.Update();
		}

		public override string ToString() => $"[BaseCharacter {Id}]";
	}

	[Serializable]
	public struct CharacterConfig
	{
		public CharacterInterest[] Interests;

		public CharacterInterest PickInterest()
		{
			return Interests[Random.Range(0, Interests.Length)];
		}
	}

	[Serializable]
	public class CharacterInterest
	{
		public float Percentage;
		public BuildingEnum Type;
		public float Radius;

		public override string ToString()
		{
			return $"[Interest {Type} Radius:{Radius}]";
		}
	}
}
