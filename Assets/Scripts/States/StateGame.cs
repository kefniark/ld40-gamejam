using System;
using System.Collections;

using Assets.Scripts.Components.StateMachine;

using UnityEngine;

namespace Assets.Scripts.States
{
	public class StateGame : State<GameStates, GameController>
	{
		private float wait = 12f;
		private bool run;

		public StateGame()
		{
			Entered += StateGame_Entered;
			Exited += StateGame_Exited;

			SetAllSlotActive(false);
		}

		private void SetAllSlotActive(bool value)
		{
			foreach (Slot slot in Slot.Slots)
			{
				slot.IsInteractable = value;
			}
		}

		private void StateGame_Entered(object sender, System.EventArgs e)
		{
			run = true;
			Entity.StartCoroutine(Spawner());
			SetAllSlotActive(true);
		}

		private void StateGame_Exited(object sender, EventArgs e)
		{
			run = false;
			SetAllSlotActive(false);
		}

		public void GameOver()
		{
			if (!IsActive)
			{
				return;
			}
			StateMachine.ChangeState(GameStates.Result);
		}

		private IEnumerator Spawner()
		{
			while (Entity.Popularity > 0.01f && run)
			{
				Entity.SpawnHouse();

				yield return new WaitForSeconds(wait);
				wait = Math.Max(wait - 1, 2f);
			}
		}
	}
}
