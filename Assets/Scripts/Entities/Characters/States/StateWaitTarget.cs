using Assets.Scripts.Components.StateMachine;
using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Characters
{
	public class StateWaitTarget : State<CharacterActionEnum, BaseCharacter>
	{
		public event EventHandler WaitChanged;
		private float stateEntered;
		public float maxDuration = 12f;
		public float Wait
		{
			get
			{
				return Math.Max(Math.Min((Time.time - stateEntered) / maxDuration, 1), 0);
			}
		}

		public StateWaitTarget()
		{
			Entered += OnStateEntered;
		}

		private void OnStateEntered(object sender, System.EventArgs e)
		{
			stateEntered = Time.time;
			WaitChanged?.Invoke(this, EventArgs.Empty);
		}

		public override void Update()
		{
			var target = Entity.GoToTarget();
			if (target != null)
			{
				WaitChanged?.Invoke(this, EventArgs.Empty);
				StateMachine.ChangeState(CharacterActionEnum.MoveToTarget);
				return;
			}

			if (Wait < 1)
			{
				WaitChanged?.Invoke(this, EventArgs.Empty);
				return;
			}

			Entity.Upset();
			StateMachine.ChangeState(CharacterActionEnum.Decide);
		}
	}
}
