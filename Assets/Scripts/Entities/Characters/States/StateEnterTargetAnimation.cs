using Assets.Scripts.Components.StateMachine;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities.Characters
{
	public class StateEnterTargetAnimation : State<CharacterActionEnum, BaseCharacter>
	{
		public StateEnterTargetAnimation()
		{
			Entered += OnStateEntered;
		}

		private void OnStateEntered(object sender, System.EventArgs e)
		{
			Entity.StartCoroutine(WaitAnimation());
		}

		private IEnumerator WaitAnimation()
		{
			yield return new WaitForSeconds(0.2f);
			Entity.SolvedInterest();
			StateMachine.ChangeState(CharacterActionEnum.LeaveTargetAnimation);
		}
	}
}
