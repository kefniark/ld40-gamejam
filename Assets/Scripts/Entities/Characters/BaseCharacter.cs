using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Building;

using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities.Characters
{
	public abstract class BaseCharacter : MonoBehaviour
	{
		public event EventHandler WaitChanged;
		public float Wait
		{
			get
			{
				if (Action != CharacterActionEnum.Idle)
				{
					return 0;
				}
				return Math.Max(Math.Min((Time.time - LastDecision) / MaxWait, 1), 0);
			}
		}

		public event EventHandler InterestReached;
		public event EventHandler InterestChanged;

		public float MaxWait = 10;
		public event EventHandler CharacterUpseted;
		protected float LastDecision;
		public int Id;
		public CharacterConfig Config;
		public NavMeshAgent NavMeshAgent;
		public CharacterActionEnum Action;
		public BaseBuilding Origin;
		public CharacterInterest Interest;
		public BaseBuilding Target;

		public void Setup(BaseBuilding house)
		{
			Id = Random.Range(0, 99999);
			Origin = house;

			Debug.Log($"{this} Setup");
			Action = CharacterActionEnum.Idle;
			NavMeshAgent.enabled = true;
		}

		#region Decision

		protected void SolvedInterest()
		{
			Debug.LogWarning($"{this} Completed her need of {Interest}");
			InterestReached?.Invoke(this, EventArgs.Empty);
			Interest = null;
		}

		protected void DecideInterest()
		{
			if (Interest != null)
			{
				return;
			}
			
			Interest = Config.PickInterest();
			LastDecision = Time.realtimeSinceStartup;
			Debug.LogWarning($"{this} Want {Interest}");

			InterestChanged?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		#region Destination

		private bool AgentHasReachedDestination()
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
			Debug.Log($"{this} FindHome {Origin}");
			return Origin;
		}

		protected BaseBuilding FindTarget()
		{
			List<BaseBuilding> slots = Slot.Slots
				.Where(x => x.Building != null && x.Building.Type == Interest.Type)
				.Where(x => Vector3.Distance(x.Building.transform.position, transform.position) < Interest.Radius)
				.Select(x => x.Building)
				.ToList();
			if (slots.Count == 0)
			{
				return null;
			}

			var rnd = new System.Random();
			int r = rnd.Next(slots.Count);

			Debug.Log($"{this} FindTarget {slots[r]}");
			return slots[r];
		}

		#endregion

		public void Update()
		{
			if (Action == CharacterActionEnum.None)
			{
				return;
			}

			if ((Time.time - LastDecision) > 10 && Target == null)
			{
				Debug.Log($"{this} Upseted");
				CharacterUpseted?.Invoke(this, EventArgs.Empty);
				Interest = null;
			}

			WaitChanged?.Invoke(this, EventArgs.Empty);

			if (Action == CharacterActionEnum.Target && AgentHasReachedDestination())
			{
				SolvedInterest();
				Target = FindHome();
				if (Target != null)
				{
					NavMeshAgent.destination = Target.Door.transform.position;
					Action = CharacterActionEnum.Home;
				}
			}
			else if (Target == null || (Action == CharacterActionEnum.Home && AgentHasReachedDestination()))
			{
				DecideInterest();
				Target = FindTarget();
				if (Target != null)
				{
					NavMeshAgent.destination = Target.Door.transform.position;
					Action = CharacterActionEnum.Target;
				}
			}
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
